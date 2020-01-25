

SET IMPORT_PATH=../proto/

SET PY_OUT_PATH=../src/python

SET CSHARP_OUT_PATH=../src/client

SET JAVA_OUT_PATH=../src/java


pushd ToolBin

::gen C# protobuf
::protogen.exe  -i:%cd%/../proto/data.proto -o:%CSHARP_OUT_PATH%\Data.cs 
::IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%
::gen pythone
protoc.exe ../proto/*.proto --python_out=%PY_OUT_PATH% -I=%IMPORT_PATH%  --csharp_out=%CSHARP_OUT_PATH% 
::--java_out=%JAVA_OUT_PATH%
IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%
::export excel
ExcelOut.exe dir:../config namespace:LL.config  exportJson:../src/json/ exportCs:../src/client/Config.cs ex:*.xlsx mode:csharp exportType:client debug:false
IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%


PServicePugin.exe dir:../proto  file:RPCMessage.proto saveto:../src/client/ exportType:client debug:false
IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%

popd

C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc /lib:%cd%/ToolBin /reference:Google.Protobuf.dll  /warn:1 /doc:%CD%\src\dll\LL.proto.xml /target:library /out:%CD%\src\dll\LL.proto.dll %cd%\src\client\*.cs
IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%
::%cd%\precompile\precompile.exe %CD%\src\dll\LL.proto.dll   -p:%cd%/ToolBin -o:%CD%\src\dll\LL.protoSerializer.dll -t:LL.ProtoSerializer
::IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%
cd ../
Copy %cd%\PublicTools\src\dll\* %cd%\UnityProject\Assets\Plugins\protolib\  /Y
Copy %cd%\PublicTools\src\dll\* %cd%\AvaterUnity\Assets\Plugins\protolib\  /Y
Copy %cd%\PublicTools\src\json\*.json %cd%\UnityProject\Assets\StreamingAssets\configs\ /Y

pause 