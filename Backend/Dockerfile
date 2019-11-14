FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY ["LSSDInOut-API/LSSDInOut-API.csproj", "LSSDInOut-API/"]
COPY ["LSSDInOut-Data/LSSDInOut-Data.csproj", "LSSDInOut-Data/"]
COPY ["LSSDInOut-Lib/LSSDInOut-Lib.csproj", "LSSDInOut-Lib/"]
RUN dotnet restore "LSSDInOut-API/LSSDInOut-API.csproj"
COPY . .
WORKDIR "/src/LSSDInOut-API"
RUN dotnet build "LSSDInOut-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LSSDInOut-API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LSSDInOut-API.dll"]