



pushd ToolBin

PServicePugin.exe dir:../proto  file:RPCMessage.proto saveto:../src/client/ exportType:client debug:false
IF not %ERRORLEVEL% == 0 exit  %ERRORLEVEL%

popd
pause 