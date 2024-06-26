#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DispatchesApp/DispatchesApp.csproj", "DispatchesApp/"]
RUN dotnet restore "DispatchesApp/DispatchesApp.csproj"
COPY . .
WORKDIR "/src/DispatchesApp"
RUN dotnet build "DispatchesApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DispatchesApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DispatchesApp.dll"]