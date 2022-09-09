FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["LexEngine.Service/LexEngine.Service.csproj", "LexEngine.Service/"]
COPY ["LexEngine/LexEngine.csproj", "LexEngine/"]

RUN dotnet restore "LexEngine.Service/LexEngine.Service.csproj"
COPY . .
WORKDIR "/src/LexEngine.Service"
RUN dotnet build "LexEngine.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LexEngine.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LexEngine.Service.dll"]
