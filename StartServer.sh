cd ./Server/GServer/

mono ./LoginServer/bin/Debug/LoginServer.exe>LoginServer.txt&
echo "LoginServer started"
sleep 1
mono ./GServer/bin/Debug/GServer.exe>GServer.txt&
echo "GServer server 1 started"
echo "Start server completed"
