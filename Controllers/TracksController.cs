using System;
using System.Threading.Tasks;
using iFoodOpenWeatherSpotify.Services;
using Microsoft.AspNetCore.Mvc;

namespace iFoodOpenWeatherSpotify.Controllers
{
  [ApiController]
  [Route("v1/")]
  public class WeatherForecastController : ControllerBase
  {
    private readonly OpenWeatherService openWeatherService;
    private readonly SpotifyService spotifyService;

    public WeatherForecastController(OpenWeatherService openWeatherService, SpotifyService spotifyService)
    {
        this.openWeatherService = openWeatherService;
        this.spotifyService = spotifyService;
    }

	[HttpGet("{city}")]
	public async Task<dynamic> GetTracksSuggestions(string city)
	{
        var forecast = await openWeatherService.GetCurrentWeatherAsync(city);
        decimal temperature = forecast.main.temp;

        await spotifyService.Authentication();
        string genre = "";

        switch (temperature)
        {
            case > 30:
                genre = "party";
                break;
            case > 15:
                genre = "pop";
                break;
            case > 10:
                genre = "rock";
                break;
            case < 10:
                genre = "classical";
                break;
        };

        var tracks = await spotifyService.GetTracksByGenreAsync(genre);

        return new {
            temperature = forecast.main.temp,
            city = forecast.name,
            country = forecast.sys.country,
            tracks = tracks.items
        };
	}
  }
}
