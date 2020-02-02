using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EConfig;
using ExcelConfig;
using GateServer;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using org.vxwo.csharp.json;
using Proto;
using Proto.MongoDB;
using ServerUtility;
using XNet.Libs.Net;
using XNet.Libs.Utility;

namespace GServer.Managers
{

   
    /// <summary>
    /// 管理用户的数据 并且管理持久化
    /// </summary>
    [Monitor]
    public class UserDataManager:IMonitor
    {

 
        private static Random random = new Random();

        private static bool Probability10000(int pro)
        {
            return random.Next(10000) < pro;
        }

       

        #region the monitor mothed
        public void OnExit()
        {
            
        }

        public void OnShowState()
        {
            //Debuger.Log("Totoal Cached User:" + userDatas.Count);
        }

        public void OnStart()
        {
            //UserData data;
            //TryToGetUserData(4, out data);
            //throw new NotImplementedException();
        }

        public void OnTick()
        {
           
        }
        #endregion


        public async Task<GameHeroEntity> FindHeroByPlayerId(string player_uuid)
        {
           
            var filter = Builders<GameHeroEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);
            var query = await  DataBase.S.Heros.FindAsync(filter);

            return query.Single();
        }

        internal async Task SyncToClient(Client userClient)
        {
            var playerUuid = (string)userClient.UserState;
            var player = await FindPlayerById(playerUuid);
            var p = await FindPackageByPlayerID(playerUuid);
            var h = (await FindHeroByPlayerId(playerUuid)).ToDhero(p);
            userClient.CreateTask<Task_G2C_SyncHero>(GateServerTask.S.SyncHero)
                .Send(() =>
                {
                    return new Task_G2C_SyncHero
                    {
                        Hero = h
                    };
                });
            userClient.CreateTask<Task_G2C_SyncPackage>(GateServerTask.S.SyncPackage)
                .Send(() =>
                {
                    return new Task_G2C_SyncPackage
                    {
                        Coin = player.Coin,
                        Gold = player.Gold,
                        Package = p.ToPackage()
                    };
                });
        }

        public async Task<GamePlayerEntity> FindPlayerById(string player_uuid)
        {
            var filter = Builders<GamePlayerEntity>.Filter.Eq(t => t.Uuid, player_uuid);
            var query = await DataBase.S.Playes.FindAsync(filter);
            return query.Single();
        }

        public async Task<GamePlayerEntity> FindPlayerByAccountId(string account_uuid)
        {
            var filter = Builders<GamePlayerEntity>.Filter.Eq(t => t.AccountUuid, account_uuid);
            var query = await DataBase.S.Playes.FindAsync(filter);
            return query.SingleOrDefault();
        }

        public async Task<ItemPackageEntity> FindPackageByPlayerID(string player_uuid)
        {
            var filter = Builders<ItemPackageEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);
            var query = await DataBase.S.Packages.FindAsync(filter);
            return query.Single();
        }

        public async Task<bool> HavePlayer(string account_uuid)
        {
            var fiter = Builders<GamePlayerEntity>.Filter.Eq(t => t.AccountUuid, account_uuid);
            var query = await DataBase.S.Playes.FindAsync(fiter);
            return query.Any();
        }

        public async Task<bool> TryToCreateUser(string userID, int heroID, string heroName)
        {
           
            var fiter = Builders<GamePlayerEntity>.Filter.Eq(t => t.AccountUuid, userID);
            var fiterHero = Builders<GameHeroEntity>.Filter.Eq(t => t.HeroName, heroName);
            //var heros = db.GetCollection<GameHeroEntity>(Hero);
            

            if(
               /*user create*/ (await DataBase.S.Playes.FindAsync(Builders<GamePlayerEntity>.Filter.Eq(t => t.AccountUuid, userID))).Any() ||
               /*hero name  */ (await DataBase.S.Heros.FindAsync(Builders<GameHeroEntity>.Filter.Eq(t => t.HeroName, heroName))).Any()
            )
            {
                
                return false;
            }

            var player = new GamePlayerEntity
            {
                AccountUuid = userID,
                Coin = 0,
                Gold = 0,
                LastIp = string.Empty
            };
            await DataBase.S.Playes.InsertOneAsync(player);
            
            var hero = new GameHeroEntity
            {
                Exp = 0,
                HeroName = heroName,
                Level = 1,
                PlayerUuid = player.Uuid,
                HeroId = heroID
            };

            await DataBase.S.Heros.InsertOneAsync(hero);

            var package = new ItemPackageEntity
            {
                PackageSize = 20,
                PlayerUuid = player.Uuid
            };

            
            await DataBase.S.Packages.InsertOneAsync(package);

            return true;

        }


        public async Task<G2C_EquipmentLevelUp> EquipLevel(string player_uuid,string item_uuid,int level)
        {
           
            var p_filter = Builders<ItemPackageEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);
            var package =  (await DataBase.S.Packages.FindAsync(p_filter)).Single();

            if (package == null) return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };

            if (!package.Items.TryGetValue(item_uuid, out ItemNum item))
                return new G2C_EquipmentLevelUp { Code = ErrorCode.NofoundItem };

            var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.Id);
            if ((ItemType)itemconfig.ItemType != ItemType.ItEquip)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };

            //装备获取失败
            var equipconfig = ExcelToJSONConfigManager.Current.GetConfigByID<EquipmentData>(int.Parse(itemconfig.Params1));
            if (equipconfig == null)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };

            //等级不一样
            if (item.Level != level)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error };


            var levelconfig = ExcelToJSONConfigManager
                .Current
                .FirstConfig<EquipmentLevelUpData>(t =>
                {
                    return t.Level == level + 1 && t.Quility == equipconfig.Quility;
                });

            if (levelconfig == null)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.Error }; ;

            var filter = Builders<GamePlayerEntity>.Filter.Eq(t => t.Uuid, player_uuid);
            var player = (await DataBase.S.Playes
                .FindAsync(filter)).Single();

            if (levelconfig.CostGold > player.Gold || levelconfig.CostCoin > player.Coin)
                return new G2C_EquipmentLevelUp { Code = ErrorCode.NoEnoughtGold }; 

            
            if (levelconfig.CostGold > 0)
            {
                player.Gold -= levelconfig.CostGold;
                var update = Builders<GamePlayerEntity>.Update.Set(t => t.Gold, player.Gold);
                DataBase.S.Playes.UpdateOne(filter, update);
            }

            if (levelconfig.CostCoin > 0)
            {
                player.Coin -= levelconfig.CostCoin;
                var update = Builders<GamePlayerEntity>.Update.Set(t => t.Coin, player.Coin);
                DataBase.S.Playes.UpdateOne(filter, update);
            }

            if (Probability10000(levelconfig.Pro))
            {
                item.Level += 1;
                var update =Builders<ItemPackageEntity>.Update.Set(t => t.Items, package.Items);
                DataBase.S.Packages.UpdateOne(p_filter, update);
            }

            return new G2C_EquipmentLevelUp { Code = ErrorCode.Ok , Level = item.Level };
        }

        public async Task<G2C_SaleItem> SaleItem(string playerUuid, IList<C2G_SaleItem.Types.SaleItem> items)
        {
            var fiter = Builders<GamePlayerEntity>.Filter.Eq(t => t.Uuid, playerUuid);
            var fiterHero = Builders<GameHeroEntity>.Filter.Eq(t => t.PlayerUuid,playerUuid);
            var fiterPackage = Builders<ItemPackageEntity>.Filter.Eq(t => t.PlayerUuid, playerUuid);

            var h = (await DataBase.S.Heros.FindAsync(fiterHero)).Single();
            var p = (await DataBase.S.Packages.FindAsync(fiterPackage)).Single();
            var pl = (await DataBase.S.Playes.FindAsync(fiter)).Single();

            foreach (var i in items)
            {
                foreach (var e in h.Equips)
                {
                    if (i.Guid == e.Value)
                        return new G2C_SaleItem { Code = ErrorCode.IsWearOnHero };
                }

                if (!p.Items.TryGetValue(i.Guid, out ItemNum item))
                    return new G2C_SaleItem { Code = ErrorCode.NofoundItem };
                if (item.Num < i.Num)
                    return new G2C_SaleItem { Code = ErrorCode.NoenoughItem };

                if (item.IsLock)
                    return new G2C_SaleItem { Code = ErrorCode.Error };

            }

            var total = 0;
            foreach (var i in items)
            {
                if (p.Items.TryGetValue(i.Guid, out ItemNum item))
                {
                    var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.Id);
                    if (config.SalePrice>0) total += config.SalePrice * i.Num;
                    item.Num -= i.Num;
                    if (item.Num == 0)
                    {
                        p.Items.Remove(i.Guid);
                    }
                }
            }

            pl.Gold += total;

            var u_player = Builders<GamePlayerEntity>.Update.Set(t => t.Gold, pl.Gold);
            var u_package = Builders<ItemPackageEntity>.Update.Set(t => t.Items, p.Items);

            await DataBase.S.Playes.UpdateOneAsync(fiter, u_player);
            await DataBase.S.Packages.UpdateOneAsync(fiterPackage, u_package);

            return new G2C_SaleItem { Code = ErrorCode.Ok, Coin = pl.Coin, Gold = pl.Gold };

        }

        internal async Task<bool> OperatorEquip(string player_uuid, string equip_uuid, EquipmentType part, bool isWear)
        {
           
            var h_filter = Builders<GameHeroEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);

            var hero = (await DataBase.S.Heros.FindAsync(h_filter)).SingleOrDefault();
            if (hero == null) return false;

            var pa_filter = Builders<ItemPackageEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);
            var package = (await DataBase.S.Packages.FindAsync(pa_filter)).Single();

            if (!package.Items.TryGetValue(equip_uuid, out ItemNum item)) return false;

            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemData>(item.Id);
            if (config == null) return false;

            var equipConfig = ExcelToJSONConfigManager.Current.GetConfigByID<EquipmentData>(int.Parse( config.Params1));
            if (equipConfig == null) return false;
            if (equipConfig.PartType != (int)part) return false;

            hero.Equips.Remove((int)part);
            if (isWear)
            {
                hero.Equips.Add((int)part, item.Uuid);
            }

            var update = Builders<GameHeroEntity>.Update.Set(t => t.Equips, hero.Equips);
            await DataBase.S.Heros.UpdateOneAsync(h_filter, update);

            return true;
        }

        [Obsolete("todo")]
        internal async Task<bool> ProcessItem(string player_uuid, int gold, Dictionary<int, PlayerItem> diff)
        {
            
            var pa_filter = Builders<ItemPackageEntity>.Filter.Eq(t => t.PlayerUuid, player_uuid);
            var package = (await DataBase.S.Packages.FindAsync(pa_filter)).Single();
            var fiter = Builders<GamePlayerEntity>.Filter.Eq(t => t.Uuid, player_uuid);
            var player = (await DataBase.S.Playes.FindAsync(fiter)).Single();
            //todo 
            return true;
        }
    }
}

