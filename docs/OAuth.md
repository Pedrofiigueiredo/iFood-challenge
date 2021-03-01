# Autenticação na API do Spotify usando OAuth

``` cs

  public async Task<string> Authentication()
      {
        var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{settings.SpotifyClientId}:{settings.SpotifyClientSecret}"));
        var bodyParams = new NameValueCollection();
        bodyParams.Add("grant_type", "client_credentials");

        var webClient = new WebClient();
        webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

        var tokenResponse = await webClient.UploadValuesTaskAsync("https://accounts.spotify.com/api/token", bodyParams);
        var textResponse = Encoding.UTF8.GetString(tokenResponse);

        var jsonResponse = JsonConvert.DeserializeObject<Token>(textResponse);

        httpClient.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", jsonResponse.access_token);

        return jsonResponse.access_token;
      }

```