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

	[HttpGet("")]
	public async Task<dynamic> Get()
	{
       await spotifyService.Authentication();

       var tracks = await spotifyService.GetTracksByGenreAsync("pop");
       return tracks.items;
	}
  }
}
