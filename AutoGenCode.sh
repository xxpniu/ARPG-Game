cd ./ToolBin
mono ./ExcelOut.exe dir:../ExcelConfig namespace:ExcelConfig  exportJson:../client/Assets/Resources/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx

mono ./ProtoParser.exe dir:../Net file:excelconst.proto saveto:../src/NetSources/ExcelConst.cs
mono ./ProtoParser.exe dir:../Net file:const.proto saveto:../src/NetSources/GameConst.cs
mono ./ProtoParser.exe dir:../Net file:data.proto saveto:../src/NetSources/GameData.cs
mono ./ProtoParser.exe dir:../Net file:Message.proto saveto:../src/NetSources/GameMessage.cs
mono ./ProtoParser.exe dir:../Net file:ServerMessage.proto saveto:../src/NetSources/ServerGameMessage.cs
mono ./ProtoParser.exe type:handle dir:../Net file:Message.proto,ServerMessage.proto saveto:../src/NetSources/NetMessageMapping.cs


mono ./DbMetal.exe -provider=MySql -database:Game_DB -server:127.0.0.1 -user:root -password:54249636 -namespace:DataBaseContext -code:GameDB.cs -sprocs
mono ./DbMetal.exe -provider=MySql -database:Game_Account_DB -server:127.0.0.1 -user:root -password:54249636 -namespace:DataBaseContext -code:GameAccountDB.cs -sprocs

echo "===>>> Begin Copy Files"

cp -af ./GameDB.cs  ../src/DB/
cp -af ./GameAccountDB.cs ../src/DB/

rm ./GameDB.cs 
rm ./GameAccountDB.cs

#不生成协议
#mcs ../src/NetSources/*.cs -warn:1 -target:library -out:../src/output/Proto.dll -doc:../src/output/Proto.xml  
echo "===>>> Begin Copy Configs To Server."
cd ../
cp -af ./client/Assets/Resources/Json/ ./Server/Configs
cp -af ./client/Assets/Resources/Layouts/ ./Server/Layouts
cp -af ./client/Assets/Resources/Magics/ ./Server/Magics
cp -af ./client/Assets/Resources/AI/ ./Server/AI

echo "===>>> SUCCESS"
