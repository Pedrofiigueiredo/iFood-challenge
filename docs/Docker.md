# Docker

Arquivo [Dockerfile](../Dockerfile) e criação do container da aplicação.

Primeiro, é interessante desabilitar o `Https Redirection` para o ambiente de produção, porque isso não será responsabilidade da aplicação, mas sim do serviço utilizado no deploy.

``` cs
  if (env.IsDevelopment())
  {
    app.UseHttpsRedirection();
  }
```

Para gerar o Dockerfile é interessante usar a extensão Docker para o VSCode, que vai criar o arquivo com as configurações mais atualizadas.

1. `Ctrl + Shift + P`
2. `docker: add dockerfile`
  * ASP.NET Core
  * Linux
  * Porta 80
  * Não gera arquivo docker-compose, a princípio


No *Dockerfile* cada linha é um set de instruções que são aplicadas à imagem que será construída:

``` Dockerfile
  # [Base stage]
  # Dependencias para a imagem ser construída com ASP.NET Core
  FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

  # "First stage" em que será construída a imagem
  WORKDIR /app

  # Expoe na porta 80
  EXPOSE 80

  # [Build stage]
  # Usa a imagem do .NETSDK, com as ferramentas para rodar uma app .NET
  # Essa imagem é maior, mais pesada, porque tem mais arquivos
  FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

  # Esse arquivos vão para o diretorio /src
  WORKDIR /src

  # Copia o arquivo .csproj, que tem todas as informações do projeto na raiz do diretorio em que estamos
  COPY ["iFoodOpenWeatherSpotify.csproj", "./"]

  # Restore em todos os pacotes da aplicacao
  RUN dotnet restore "iFoodOpenWeatherSpotify.csproj"

  # Copia todos os demais arquivos do projeto
  COPY . .

  WORKDIR "/src/."

  # dotnet build: o script do .NET para construir a app no ambiente de produção
  # -c Release: flag para uma Release version do app
  # - o /app/build: flag que define para onde vai o resultado dessa build
  RUN dotnet build "iFoodOpenWeatherSpotify.csproj" -c Release -o /app/build

  # [publish stage]
  # Do stage build, que acabou de ser criado, para o stage publish
  FROM build AS publish

  # Comando padrão do .NET, com as mesmas flags do comando build
  RUN dotnet publish "iFoodOpenWeatherSpotify.csproj" -c Release -o /app/publish

  # [final stage]
  FROM base AS final
  WORKDIR /app

  # Copia tudo que está no stage build para /app/publish
  COPY --from=publish /app/publish .

  # Como iniciar a REST API
  # Nesse caso, executa o comando dotnet com o arquivo .dll
  ENTRYPOINT ["dotnet", "iFoodOpenWeatherSpotify.dll"]

```

Cada serviço de deploy pode ter uma configuração diferente para o Dockerfile, com alguns detalhes específicos para aquele ambiente.

No *Heroku*, por exemplo, há uma diferença:

``` Dockerfile
  # Forma para o Heroku
  CMD ASPNETCORE_URLS=http://*:$PORT dotnet iFoodOpenWeatherSpotify.dll
```

Então, para *buildar* a imagem do docker:

`docker build -t [name:tag] .`

em que `-t` é uma tag para o nome da imagem e uma tag para ela