ROOT=./Server/GServer

PATH=$ROOT/LoginServer/bin/Debug/netcoreapp2.1
echo $PATH

mono $PATH/LoginServer.dll>LoginServer.txt&
if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi
echo "LoginServer started"
