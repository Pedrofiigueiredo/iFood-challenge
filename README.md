# iFood Backend Challenge

&#9881; Em desenvolvimento

Esse é um desafio backend proposto pelo iFood, usando o Open Weather Map e a API do Spotify.
[Link do repositório original](https://github.com/ifood/vemproifood-backend)

## Index

- [iFood Backend Challenge](#ifood-backend-challenge)
  - [Index](#index)
  - [Tecnologias e ferramentas](#tecnologias-e-ferramentas)
  - [Rotas e exemplos](#rotas-e-exemplos)
  - [Regras de negócio](#regras-de-negócio)
  - [Como rodar localmente](#como-rodar-localmente)

## Tecnologias e ferramentas

* .NET 5
* [Open Weather Map API](https://openweathermap.org/)
* [Spotify API](https://developer.spotify.com/)
* HttpClient e WebClient
* OAuth 2.0
* Docker

## Rotas e exemplos

* `GET v1/:city` - return tracks suggestions based on weather forecast.

Requisição: `GET v1/campinas`
```
{
  "temperature": 23.16,
  "city": "Campinas",
  "country": "BR",
  "tracks": [
    {
      "track": {
        "name": "test drive",
        "href": "https://api.spotify.com/v1/tracks/3eZYOQO4UzKrUDYDghtnFw",
        "artists": [
          {
            "name": "Ariana Grande"
          }
        ]
      }
    },
    {
      "track": {
        "name": "Save Your Tears",
        "href": "https://api.spotify.com/v1/tracks/5QO79kh1waicV47BqGRL3g",
        "artists": [
          {
            "name": "The Weeknd"
          }
        ]
      }
    },

    ...
```

## Regras de negócio

* `Temperatura > 30` - músicas para festa (`spotify genreId: party`)
* `15 >= Temperatura >= 30` - músicas Pop (`spotify genreId: pop`)
* `10 >= Temperatura > 15` - músicas de Rock (`spotify genreId: rock`)
* `Temperatura < 10` - músicas classicas (`spotify genreId: classical`)

## Como rodar localmente

1. Faça um clone desse repositório
   `git clone https://github.com/Pedrofiigueiredo/iFood-challenge`

2. Configure as variáveis ambiente pelo termial

   `dotnet user-secrets init`  
   `dotnet user-secrets set ServiceSettings:OpenWeatherApiKey {SUA_CHAVE}`  
   `dotnet user-secrets set ServiceSettings:SpotifyClientId {SEU_ID}`
   `dotnet user-secrets set ServiceSettings:SpotifyClientSecret {SEU_SECRET}`
   
   `obs`: Vocẽ pode usar o comando `dotnet user-secrets list` para ver as variáveis adicionadas.

3. Para rodar utilize o comando `dotnet run`. O servidor irá iniciar em `https://localhost:5001`.


`Obs1:` Para obter as keys para conectar com as APIs você terá que logar e copiá-las na aba de dashboard.
