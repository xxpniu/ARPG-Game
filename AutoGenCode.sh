cd ./ToolBin
mono ./ExcelOut.exe dir:../ExcelConfig namespace:ExcelConfig  exportJson:../client/Assets/Resources/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx

mono ./ProtoParser.exe dir:../Net file:excelconst.proto saveto:../src/NetSources/ExcelConst.cs
mono ./ProtoParser.exe dir:../Net file:const.proto saveto:../src/NetSources/GameConst.cs
mono ./ProtoParser.exe dir:../Net file:data.proto saveto:../src/NetSources/GameData.cs
mono ./ProtoParser.exe dir:../Net file:Message.proto saveto:../src/NetSources/GameMessage.cs
mono ./ProtoParser.exe type:handle dir:../Net file:Message.proto saveto:../src/NetSources/NetMessageMapping.cs

cd ../
cp -af ./client/Assets/Resources/Json/ ./Server/Configs
cp -af ./client/Assets/Resources/Layouts/ ./Server/Layouts
cp -af ./client/Assets/Resources/Magics/ ./Server/Magics
cp -af ./client/Assets/Resources/AI/ ./Server/AI

echo "SUCCESS"
