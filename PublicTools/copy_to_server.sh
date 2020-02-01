cd ./ToolBin

cho "===>>> Begin Copy Configs To Server."
cd ../
cp -af ./client/Assets/Resources/Json/ ./Server/Configs
cp -af ./client/Assets/Resources/Layouts/ ./Server/Layouts
cp -af ./client/Assets/Resources/Magics/ ./Server/Magics
cp -af ./client/Assets/Resources/AI/ ./Server/AI

echo "===>>> SUCCESS"
