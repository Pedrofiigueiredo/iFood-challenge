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

	[HttpGet("{:city}")]
	public async Task<dynamic> GetTracksSuggestions(string city)
	{
        var temperature = await openWeatherService.GetCurrentWeatherAsync(city);
        await spotifyService.Authentication();

        switch (temperature)
        {
            case > 30:
                return await spotifyService.GetTracksByGenreAsync("party");
            case > 15:
                return await spotifyService.GetTracksByGenreAsync("pop");
            case > 10:
                return await spotifyService.GetTracksByGenreAsync("rock");
            case < 10:
                return await spotifyService.GetTracksByGenreAsync("classical");
        };

        return new {
            error = "Houve um erro ao buscar músicas"
        };
	}
  }
}
