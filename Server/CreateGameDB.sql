#create database Game_DB;
#游戏数据库
#use Game_DB;

#玩家数据表
create table `TB_GamePlayer`
(
   `UserID` bigint primary key,
   `UserPackage` text ,
   `Gold` int default 0,
   `Coin` int default 0
);

#玩家装备特性表
create table `TB_PlayerEquip`
(
   `UserID` bigint primary key,
   `UserEquipValues` text comment '装备养成数据'
);

#玩家角色表
create table `TB_PlayerHero`
(
   `ID` bigint auto_increment primary key, 
   `UserID` bigint ,
   `HeroID` int,
   `Level` int,
   `Exp` int default 0 comment '角色当前经验',
   `Magics` text  comment '英雄养成数据'
);

