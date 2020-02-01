
ROOT=./Server/GServer

dotnet $ROOT/LoginServer/bin/Debug/netcoreapp2.1/LoginServer.dll>LoginServer.txt&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi
sleep 3

dotnet $ROOT/GServer/bin/Debug/netcoreapp2.1/GateServer.dll>GServer.txt&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi
sleep 3

dotnet $ROOT/MapServer/bin/Debug/netcoreapp2.1/MapServer.dll>MapServer.txt&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi

#ps aux | grep donet 
#kill 2