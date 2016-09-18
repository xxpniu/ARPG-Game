using System;
using System.Linq;
using DataBaseContext;
using org.vxwo.csharp.json;
using Proto;
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

        private SyncDictionary<long, UserData> userData = new SyncDictionary<long, UserData>();

        #region the monitor mothed
        public void OnExit()
        {
            using (var db = Appliaction.Current.GetDBContext() )
            {
                foreach (var i in userData)
                {
                    SaveUser(i.Key, i.Value,db);
                }

                db.SubmitChanges();
            }
            //throw new NotImplementedException();
        }

        public void OnShowState()
        {
            Debuger.Log("Totoal Cached User:" + userData.Count);
        }

        public void OnStart()
        {
            //UserData data;
            //TryToGetUserData(4, out data);
            //throw new NotImplementedException();
        }

        private DateTime lastTick = DateTime.Now;

        public void OnTick()
        {
            if ((DateTime.Now - lastTick).TotalSeconds > 60)
            {
                lastTick = DateTime.Now;
                using (var db = Appliaction.Current.GetDBContext())
                {
                    foreach (var i in userData)
                    {
                        if (i.Value.IsDead)
                        {
                            SaveUser(i.Key, i.Value, db);
                            userData.Remove(i.Key);
                        }
                    }
                    db.SubmitChanges();
                }
            }
        }
        #endregion

        #region SaveData To DB

        private void SaveUser(long userID, UserData data, DataBaseContext.GameDb db)
        {
            if (data.IsChanged)
            {
                SavePackage(userID, data.GetPackage(), data.Gold, data.Coin, db);
            }
            if (data.IsEquipChanged)
            {
                SaveEquip(userID, data.GetPackage(), db);
            }

            if (data.IsHeroChanaged)
            {
                SaveHero(userID, data.GetHero(), db);
            }
            data.Pristed();
        }

        void SaveHero(long userID, DHero dHero, GameDb db)
        {
            var hero = db.TBPLayerHero.Where(t => t.UserID == userID).SingleOrDefault();
            if (hero == null) return;
            hero.Exp = dHero.Exprices;
            hero.Equips = JsonTool.Serialize(dHero.Equips);
            hero.Level = dHero.Level;
            hero.Magics = JsonTool.Serialize(dHero.Magics);
        }

        private void SavePackage(long userID, Proto.PlayerPackage package, int gold, int coin, DataBaseContext.GameDb db)
        {
            var user = db.TBGAmePlayer.Where(t => t.UserID == userID)
                         .SingleOrDefault();
            if (user == null) return;
            user.UserPackage = JsonTool.Serialize(package.Items);
            user.Coin = coin;
            user.Gold = gold;
        }

        private void SaveEquip(long userID, Proto.PlayerPackage package, DataBaseContext.GameDb db)
        {
            var eqiup = db.TBPLayerEquip.Where(t => t.UserID == userID).SingleOrDefault();
            if (eqiup == null) return;
            eqiup.UserEquipValues = JsonTool.Serialize(package.Equips);
        }

        #endregion

        #region Create User 
        public bool TryToCreateUser(long userID,int heroID)
        {
            using (var db = Appliaction.Current.GetDBContext())
            {
                var query = db.TBGAmePlayer.Where(t => t.UserID == userID);
                if (query.Count() > 0)
                {
                    return false;
                }

                var gamePlayer = new TBGAmePlayer
                {
                    Coin = 100,
                    UserID = userID,
                    Gold = 100,
                    PackageSize = 50,
                    UserPackage = string.Empty
                };

                db.TBGAmePlayer.InsertOnSubmit(gamePlayer);

                var hero = new TBPLayerHero
                {
                    UserID = userID,
                    HeroID = heroID,
                    Exp = 0,
                    Level = 1,
                    Magics = string.Empty,
                    Equips = string.Empty
                };
                db.TBPLayerHero.InsertOnSubmit(hero);

                var equip = new TBPLayerEquip
                {
                    UserID = userID,
                    UserEquipValues = string.Empty
                };
                db.TBPLayerEquip.InsertOnSubmit(equip);

                db.SubmitChanges();
                return true;
            }
        }
        #endregion

        #region try to Get user data
        public bool TryToGetUserData(long userID, out UserData data, bool reload = false)
        {
            if (reload)
            {
                userData.Remove(userID);
            }

            if (!userData.TryToGetValue(userID, out data))
            {
                using (var db = Appliaction.Current.GetDBContext())
                {
                    var user = db.TBGAmePlayer.Where(t => t.UserID == userID).SingleOrDefault();
                    if (user == null)
                    {
                        return false;
                    }
                    var equip = db.TBPLayerEquip.Where(t => t.UserID == userID).SingleOrDefault();
                    var hero = db.TBPLayerHero.Where(t => t.UserID == userID).SingleOrDefault();
                    if (hero == null)
                    {
                        return false;
                    }

                    var package = DataManager.GetPackageFromTbPlayer(user, equip);
                    var dhero = DataManager.GetDHeroFromTBhero(hero);
                    data = new UserData(dhero, package, user.Gold, user.Coin);

                    return userData.Add(userID, data);
                }
            }
            else {
                data.Accessed();
                return true;
            }

        }
        #endregion
    }
}

