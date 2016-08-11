cd ./Server/GServer/


mono ./LoginServer/bin/Debug/LoginServer.exe 1900 1800 127.0.0.1 Game_Account_DB root 54249636 key001  >LoginServer.txt&
echo "LoginServer started"
#sleep 1
#mono ./GServer/bin/Debug/GServer.exe 1700  127.0.0.1 1800 127.0.0.1 Game_DB xxp 54249636 1 "../../../../" key001  >GServer.txt&
#echo "GServer server 1 started"
#echo "Start server completed"
