
ROOT=./Server/GServer

dotnet $ROOT/GServer/bin/Debug/netcoreapp2.1/GateServer.dll>gate_server_log.log&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi


#ps aux|grep donet  
#pkill GateServer 2  
#dotnet ./Server/GServer/GServer/bin/Debug/netcoreapp2.1/GateServer.dll
#dotnet ./Server/GServer/LoginServer/bin/Debug/netcoreapp2.1/LoginServer.dll