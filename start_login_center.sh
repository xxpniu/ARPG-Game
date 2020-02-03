ROOT=./Server/GServer

dotnet $ROOT/LoginServer/bin/Debug/netcoreapp2.1/LoginServer.dll>login_server_log.log&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi
echo "LoginServer started"
