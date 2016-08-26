echo "===>>> Copy dll to client."
cp -af ./GameCore/GameLogic/bin/Release/  ./client/Assets/Plugins/
cp -af ./GameCore/XNet/bin/Release/  ./client/Assets/Plugins/
cp -af ./Server/GServer/Astar/bin/Release ./client/Assets/Plugins/
echo "===>>> Copy Configs TO Server."
cp -af ./client/Assets/Resources/Json/ ./Server/Configs
cp -af ./client/Assets/Resources/Layouts/ ./Server/Layouts
cp -af ./client/Assets/Resources/Magics/ ./Server/Magics
cp -af ./client/Assets/Resources/AI/ ./Server/AI
echo "===>>> success."