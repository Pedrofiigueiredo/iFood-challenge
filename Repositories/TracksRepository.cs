using System.Threading.Tasks;
using tracker.Services;

namespace tracker.Repositories
{
  public class TracksRepository
  {
    private readonly OpenWeatherService openWeatherService;
    private readonly SpotifyService spotifyService;
    public TracksRepository(OpenWeatherService openWeatherService, SpotifyService spotifyService)
    {
        this.openWeatherService = openWeatherService;
        this.spotifyService = spotifyService;
    }

    public async Task<dynamic> GetTracksSuggestions(string city)
    {
      var forecast = await openWeatherService.GetCurrentTemperatureAsync(city);
      await spotifyService.Authentication();
      string genre = "";
      switch (forecast.Temperature)
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
        temp = forecast.Temperature,
        city,
        country = forecast.Country,
        tracks = tracks.items
      };
    }
  }
}