using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace iFoodOpenWeatherSpotify.Services
{
  public class OpenWeatherService
  {
    private readonly ServiceSettings settings;
    private readonly HttpClient httpClient;

    public OpenWeatherService(IOptions<ServiceSettings> options, HttpClient httpClient)
    {
      this.settings = options.Value;
      this.httpClient = httpClient;
    }

    public record Main(decimal temp);
    public record Sys(string country);
    public record Forecast(Main main, string name, Sys sys);

    /// <summary> Get current weather forecast of a city. </summary>
    /// <param name="city">A string for city</param>
    /// <returns>
    ///   JSON object with current temperature, city and country.
    /// </returns>
    public async Task<Forecast> GetCurrentWeatherAsync(string city)
    {
        var forecast = await httpClient
          .GetFromJsonAsync<Forecast>(
            $"{settings.OpenWeatherHost}/data/2.5/weather?q={city}&appid={settings.OpenWeatherApiKey}&units=metric"
          );

        return forecast;
    }
  }
}
