#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Monarchy.Gateway.Test/Monarchy.Gateway.Test.csproj", "Monarchy.Gateway.Test/"]
RUN dotnet restore "Monarchy.Gateway.Test/Monarchy.Gateway.Test.csproj"
COPY . .
WORKDIR "/src/Monarchy.Gateway.Test"
RUN dotnet build "Monarchy.Gateway.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Monarchy.Gateway.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monarchy.Gateway.Test.dll"]