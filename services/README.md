# Dotnet Service

# Generate

```terminal
dotnet new sln -n DotnetService

dotnet new webapi -o AdminHotelApi -controllers
dotnet new webapi -o CustomerHotelApi -controllers
dotnet new worker -o CommandHotelWorker
dotnet new console -n TestConsole

dotnet sln DotnetService.sln add AdminHotelApi/AdminHotelApi.csproj
dotnet sln DotnetService.sln add CustomerHotelApi/CustomerHotelApi.csproj
dotnet sln DotnetService.sln add CommandHotelWorker/CommandHotelWorker.csproj
dotnet sln DotnetService.sln add TestConsole/TestConsole.csproj
```

# Developing

```terminal
dotnet restore
dotnet build
dotnet format
```
