FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["tracker.csproj", "./"]
RUN dotnet restore "tracker.csproj"
COPY . .
RUN dotnet publish "tracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Forma padrao
# ENTRYPOINT ["dotnet", "tracker.dll"]

# Forma para o Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet tracker.dll
