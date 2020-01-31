cd ./ToolBin

IMPORT_PATH=../proto/
CSHARP_PATH=../src/csharp
CSHARP_OUT_PATH=$CSHARP_PATH

protoc ../proto/*.proto --csharp_out=$CSHARP_OUT_PATH -I=$IMPORT_PATH

mono PServicePugin.exe dir:../proto file:*.proto saveto:$CSHARP_OUT_PATH debug:false

mono ./ExcelOut.exe dir:../econfigs namespace:EConfig exportJson:../src/json/ exportCs:$CSHARP_OUT_PATH/ExcelConfig.cs ex:*.xlsx

mcs $CSHARP_OUT_PATH/*.cs -warn:1 -target:library -out:../src/output/Xsoft.Proto.dll -doc:../src/output/Xsoft.Proto.xml -r:Google.Protobuf.dll

if [ "$?" -ne "0" ]; then
  echo "Sorry, execute failure"
  exit 1
fi

