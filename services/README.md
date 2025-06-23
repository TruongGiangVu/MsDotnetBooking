# Dotnet Service

# Generate

```terminal
dotnet new sln -n DotnetService

dotnet new webapi -o AdminHotelApi -controllers
dotnet new webapi -o CustomerHotelApi -controllers
dotnet new worker -o CommandHotelWorker
dotnet new classlib -n Core
dotnet new console -n TestConsole

dotnet sln DotnetService.sln add AdminHotelApi/AdminHotelApi.csproj
dotnet sln DotnetService.sln add CustomerHotelApi/CustomerHotelApi.csproj
dotnet sln DotnetService.sln add CommandHotelWorker/CommandHotelWorker.csproj
dotnet sln DotnetService.sln add TestConsole/TestConsole.csproj
dotnet sln DotnetService.sln add Core/Core.csproj

dotnet add AdminHotelApi/AdminHotelApi.csproj reference Core/Core.csproj
dotnet add CustomerHotelApi/CustomerHotelApi.csproj reference Core/Core.csproj
dotnet add CommandHotelWorker/CommandHotelWorker.csproj reference Core/Core.csproj
```

# Developing

```terminal
dotnet restore
dotnet build
dotnet format
```

# Docker
```
docker build -f Dockerfile.admin -t admin-hotel-api .

```
