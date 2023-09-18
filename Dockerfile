FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY ./neostack_backend_testtask.sln .
COPY ./neostack_backend_testtask.csproj ././neostack_backend_testtask.csproj
COPY ./appsettings.json ././appsettings.json
RUN dotnet restore

# copy everything else and build app
COPY ./. ./aspnetapp/
WORKDIR /source/aspnetapp
RUN dotnet add package Microsoft.EntityFrameworkCore.Analyzers --version 7.0.11
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://0.0.0.0:80  
ENTRYPOINT ["dotnet", "neostack_backend_testtask.dll"]