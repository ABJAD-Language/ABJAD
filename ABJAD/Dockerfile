FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["ABJAD.InterpretEngine.Service/ABJAD.InterpretEngine.Service.csproj", "ABJAD.InterpretEngine.Service/"]
COPY ["ABJAD.InterpretEngine/ABJAD.InterpretEngine.csproj", "ABJAD.InterpretEngine/"]

RUN dotnet restore "ABJAD.InterpretEngine.Service/ABJAD.InterpretEngine.Service.csproj"
COPY . .
WORKDIR "/src/ABJAD.InterpretEngine.Service"
RUN dotnet build "ABJAD.InterpretEngine.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ABJAD.InterpretEngine.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ABJAD.InterpretEngine.Service.dll"]
