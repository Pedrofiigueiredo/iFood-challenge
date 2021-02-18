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
    public record Forecast(Main main);

    public async Task<decimal> GetCurrentWeatherAsync(string city)
    {
      var forecast = await httpClient
        .GetFromJsonAsync<Forecast>(
          $"{settings.OpenWeatherHost}/data/2.5/weather?q=salvador&appid={settings.OpenWeatherApiKey}&units=metric"
        );

      return forecast.main.temp;
    }
  }
}
