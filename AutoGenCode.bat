@echo off
pushd ToolBin
ExcelOut dir:../ExcelConfig namespace:ExcelConfig  exportJson:../client/Assets/Resources/Json exportCs:../src/NetSources/Config.cs ex:*.xlsx

ProtoParser dir:../Net file:excelconst.proto saveto:../src/NetSources/ExcelConst.cs
ProtoParser dir:../Net file:const.proto saveto:../src/NetSources/GameConst.cs
ProtoParser dir:../Net file:data.proto saveto:../src/NetSources/GameData.cs
ProtoParser dir:../Net file:Message.proto saveto:../src/NetSources/GameMessage.cs

PNet dir:../Net file:netmessage.mapping saveto:../src/NetSources/NetMessageMapping.cs
popd
::C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc /warn:1 /doc:%cd%\Unity\Assets\Plugins\proto.xml /target:library /out:%cd%\Unity\Assets\Plugins\proto.dll %cd%\src\NetSources\*.cs

Copy %cd%\client\Assets\Resources\Json\*.json %cd%\Server\Configs  /Y
@::opy %cd%\client\Assets\Plugins\proto.dll %cd%\Server\Lib\  /Y
::copy %cd%\client\Assets\Plugins\proto.xml %cd%\Server\Lib\  /Y
pause