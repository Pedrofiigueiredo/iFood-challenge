using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace iFoodOpenWeatherSpotify.Services
{
  public class SpotifyService
  {
    private readonly HttpClient httpClient;
    private readonly ServiceSettings settings;

    public SpotifyService(HttpClient httpClient, IOptions<ServiceSettings> options)
    {
      this.httpClient = httpClient;
      settings = options.Value;

      var token = "BQBsNEcnwIjphGjZXFhwbuL3Qep0dsAXx5Wp67_D0qy5Ie1UzM0I5dIS8-cRmhizFBWu8VKsBduCawUE43Q0NGFlYZ9ah8sewmHVsrh3gUQvmMRbRvPVKjvwKTxZTQuewtycv-bIA5Wsz_q5vPb7no-L4BVCa0LQffo";

      httpClient.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
    }

    public record Playlists(PlaylistItems[] items);
    public record PlaylistItems(string name, string id);
    public record PlaylistsData(Playlists playlists);

    private async Task<PlaylistsData> GetPlaylistsByGenreAsync(string genre)
    {
      var playlists = await httpClient
        .GetFromJsonAsync<PlaylistsData>(
          $"{settings.SpotifyHost}/v1/browse/categories/{genre}/playlists?offset=0&limit=5"
        );

      return playlists;
    }

    public record TrackItems(Track track);
		public record Track(string name);
		public record TrackData(TrackItems[] items);

    private async Task<TrackData> GetTracksFromPlaylistAsync(string playlistId)
    {
      var tracks = await httpClient
        .GetFromJsonAsync<TrackData>(
          $"{settings.SpotifyHost}/v1/playlists/{playlistId}/tracks"
        );

			return tracks;
    }

    public async Task<TrackData> GetTracksByGenreAsync(string genre)
    {
      var res = await GetPlaylistsByGenreAsync(genre);

      var playlist = res.playlists.items[0];

      var tracks = await GetTracksFromPlaylistAsync(playlist.id);

      return tracks;
    }
  }
}