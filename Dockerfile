FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["ABJAD.LexEngine.Service/ABJAD.LexEngine.Service.csproj", "ABJAD.LexEngine.Service/"]
COPY ["ABJAD.LexEngine/ABJAD.LexEngine.csproj", "ABJAD.LexEngine/"]

RUN dotnet restore "ABJAD.LexEngine.Service/ABJAD.LexEngine.Service.csproj"
COPY . .
WORKDIR "/src/ABJAD.LexEngine.Service"
RUN dotnet build "ABJAD.LexEngine.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ABJAD.LexEngine.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ABJAD.LexEngine.Service.dll"]
