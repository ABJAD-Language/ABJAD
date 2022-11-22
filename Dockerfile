FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["ABJAD.ParseEngine.Service/ABJAD.ParseEngine.Service.csproj", "ABJAD.ParseEngine.Service/"]
COPY ["ABJAD.ParseEngine/ABJAD.ParseEngine.csproj", "ABJAD.ParseEngine/"]

RUN dotnet restore "ABJAD.ParseEngine.Service/ABJAD.ParseEngine.Service.csproj"
COPY . .
WORKDIR "/src/ABJAD.ParseEngine.Service"
RUN dotnet build "ABJAD.ParseEngine.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ABJAD.ParseEngine.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ABJAD.ParseEngine.Service.dll"]
