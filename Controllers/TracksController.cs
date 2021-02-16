using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using iFoodOpenWeatherSpotify.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

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
			return await spotifyService.GetTracksByGenreAsync("pop");
		}
	}
}
