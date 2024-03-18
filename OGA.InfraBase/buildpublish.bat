REM NET Core Software Library

REM Build the library...
dotnet restore "./OGA.InfraBase_NET5/OGA.InfraBase_NET5.csproj"
dotnet build "./OGA.InfraBase_NET5/OGA.InfraBase_NET5.csproj" -c DebugLinux --runtime linux-x64 --no-self-contained

dotnet restore "./OGA.InfraBase_NET5/OGA.InfraBase_NET5.csproj"
dotnet build "./OGA.InfraBase_NET5/OGA.InfraBase_NET5.csproj" -c DebugWin --runtime win-x64 --no-self-contained

dotnet restore "./OGA.InfraBase_NET6/OGA.InfraBase_NET6.csproj"
dotnet build "./OGA.InfraBase_NET6/OGA.InfraBase_NET6.csproj" -c DebugLinux --runtime linux-x64 --no-self-contained

dotnet restore "./OGA.InfraBase_NET6/OGA.InfraBase_NET6.csproj"
dotnet build "./OGA.InfraBase_NET6/OGA.InfraBase_NET6.csproj" -c DebugWin --runtime win-x64 --no-self-contained

dotnet restore "./OGA.InfraBase_NET7/OGA.InfraBase_NET7.csproj"
dotnet build "./OGA.InfraBase_NET7/OGA.InfraBase_NET7.csproj" -c DebugLinux --runtime linux-x64 --no-self-contained

dotnet restore "./OGA.InfraBase_NET7/OGA.InfraBase_NET7.csproj"
dotnet build "./OGA.InfraBase_NET7/OGA.InfraBase_NET7.csproj" -c DebugWin --runtime win-x64 --no-self-contained

REM Create the composite nuget package file from built libraries...
C:\Programs\nuget\nuget.exe pack ./OGA.InfraBase.nuspec -IncludeReferencedProjects -symbols -SymbolPackageFormat snupkg -OutputDirectory ./Publish -Verbosity detailed

REM To publish nuget package...
dotnet nuget push -s http://192.168.1.161:8080/v3/index.json ".\Publish\OGA.InfraBase.3.4.4.nupkg"
dotnet nuget push -s http://192.168.1.161:8080/v3/index.json ".\Publish\OGA.InfraBase.3.4.4.snupkg"

TIMEOUT 10

ECHO "DONE"
