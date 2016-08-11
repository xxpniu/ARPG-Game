
create table `TB_Account`
(
   `ID` bigint primary Key AUTO_INCREMENT,
   `UserName` varchar(255) default '',
   `Password` varchar(50) default '',
   `ServerID` int default 0,
   `CreateDateTime` datetime default now(),
   `LastLoginDateTime` datetime ,
   `LoginCount` bigint default 0
)

