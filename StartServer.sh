cd ./Server/GServer/

mono ./LoginServer/bin/Release/LoginServer.exe>LoginServer.txt&
echo "LoginServer started"
sleep 1
mono ./GServer/bin/Release/GServer.exe>GServer.txt&
echo "GServer server 1 started"
sleep 1
#mono ./MapServer/bin/Release/MapServer.exe>MapServer.txt&
echo "Start server completed"
