FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["iFoodOpenWeatherSpotify.csproj", "./"]
RUN dotnet restore "iFoodOpenWeatherSpotify.csproj"
COPY . .
RUN dotnet publish "iFoodOpenWeatherSpotify.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Forma padrao
# ENTRYPOINT ["dotnet", "iFoodOpenWeatherSpotify.dll"]

# Forma para o Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet iFoodOpenWeatherSpotify.dll
