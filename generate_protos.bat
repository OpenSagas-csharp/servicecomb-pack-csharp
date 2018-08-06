
@rem Generate the C# code for .proto files

@rem enter this directory
cd /d %~dp0

set PROTOC=%UserProfile%\.nuget\packages\google.protobuf.tools\3.6.0\tools\windows_x64\protoc.exe
set PLUGIN=%UserProfile%\.nuget\packages\grpc.tools\1.14.0\tools\windows_x64\grpc_csharp_plugin.exe

dotnet restore

%PROTOC% -I protos   --csharp_out src/Servicecomb.Saga.Omega.Protocol protos/GrpcTxEvent.proto --grpc_out src/Servicecomb.Saga.Omega.Protocol --plugin=protoc-gen-grpc=%PLUGIN%