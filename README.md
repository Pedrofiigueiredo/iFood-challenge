# iFood Backend Challenge

&#9881; Em desenvolvimento

Esse é um desafio backend proposto pelo iFood, usando o Open Weather Map e a API do Spotify.
[Link do repositório original](https://github.com/ifood/vemproifood-backend)

## Tecnologias e ferramentas

* .NET 5
* [Open Weather Map API](https://openweathermap.org/)
* [Spotify API](https://developer.spotify.com/)
* HttpClient e WebClient
* OAuth 2.0


## Docker

1. Https Redirection (habilitar somente no ambiente de desenvolvimento)
2. Gerar o Dockerfile (extensão Docker para o VSCode)
   1. Ctrl + shift + P
   2. docker: add dockerfile
      1. ASP.NET Core
      2. Linux
      3. Porta 80
      4. Não gera arquivo docker-compose
3. Dockerfile (cada linha é um set de instruções que são aplicadas a imagem que será construída)

```
// [Base stage]
// Dependencias para a imagem ser construída com ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

// "First stage" em que será construída a imagem
WORKDIR /app

// Expoe na porta 80
EXPOSE 80

// [Build stage]
// Usa a imagem do .NETSDK, com as ferramentas para rodar uma app .NET
// Essa imagem é maior, mais pesada, porque tem mais arquivos
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

// Esse arquivos vão para o diretorio /src
WORKDIR /src

// Copia o arquivo .csproj, que tem todas as informações do projeto na raiz do diretorio em que estamos
COPY ["iFoodOpenWeatherSpotify.csproj", "./"]

// Restore em todos os pacotes da aplicacao
RUN dotnet restore "iFoodOpenWeatherSpotify.csproj"

// Copia todos os demais arquivos do projeto
COPY . .


WORKDIR "/src/."

// dotnet build é um script do próprio .NET que vai construir a app  para o ambiente de produção
// -c Release é para uma Release version do app
// - o /app/build é para onde vai o resultado dessa build
RUN dotnet build "iFoodOpenWeatherSpotify.csproj" -c Release -o /app/build

// [publish stage]
// Do stage build, que acabou de ser criado, para o stage publish
FROM build AS publish

// comando padrão do .NET, com as mesmas flags do comando build
RUN dotnet publish "iFoodOpenWeatherSpotify.csproj" -c Release -o /app/publish

// [final stage]
//
FROM base AS final
WORKDIR /app

// Copia tudo que está no stage build para /app/publish
COPY --from=publish /app/publish .

// Como iniciar a REST API
// Nesse caso, executa o comando dotnet com o arquivo .dll
ENTRYPOINT ["dotnet", "iFoodOpenWeatherSpotify.dll"]

```

4. Build a imagem do docker
   `dotnet build -t [name:tag] .`
   -t é uma tag para o nome da imagem e uma tag para ela
5. 