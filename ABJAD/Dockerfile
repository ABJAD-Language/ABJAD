FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ABJAD.csproj", "ABJAD.csproj"]

RUN dotnet restore "ABJAD.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "ABJAD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ABJAD.csproj" -c Release -o /app/publish

FROM base AS final
ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ABJAD.dll"]
