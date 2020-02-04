
dotnet ./Server/GServer/LoginServer/bin/Debug/netcoreapp2.1/LoginServer.dll>login_server_log.log&
sleep 3
dotnet ./Server/GServer/GServer/bin/Debug/netcoreapp2.1/GateServer.dll>gate_server_log.log&