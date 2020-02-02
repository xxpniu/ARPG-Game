cd ./ToolBin

IMPORT_PATH=../proto/
CSHARP_PATH=../src/csharp
CSHARP_OUT_PATH=$CSHARP_PATH

protoc ../proto/*.proto --csharp_out=$CSHARP_OUT_PATH -I=$IMPORT_PATH
if [ "$?" -ne "0" ]; then
  echo "Failur  check proto files"
  exit 1
fi

mono PServicePugin.exe dir:../proto file:*.proto saveto:$CSHARP_OUT_PATH debug:false
if [ "$?" -ne "0" ]; then
  echo "Sorry, check service define"
  exit 1
fi

mono ./ExcelOut.exe dir:../econfigs namespace:EConfig exportJson:../src/json/ exportCs:$CSHARP_OUT_PATH/ExcelConfig.cs ex:*.xlsx
if [ "$?" -ne "0" ]; then
  echo "Sorry, check excel files "
  exit 1
fi

mcs $CSHARP_OUT_PATH/*.cs -warn:1 -target:library -out:../src/output/Xsoft.Proto.dll -doc:../src/output/Xsoft.Proto.xml -r:Google.Protobuf.dll
if [ "$?" -ne "0" ]; then
  echo "Sorry, compile error"
  exit 1
fi


cp -af ../src/output/  ../../client/Assets/Plugins/CoreDll/
cp -af ../src/json/  ../../client/Assets/Resources/Json/
cp -af ../src/json/  ../../Server/Configs/

if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi

