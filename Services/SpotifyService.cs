using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace tracker.Services
{
  public class SpotifyService
  {
    private readonly HttpClient httpClient;
    private readonly ServiceSettings settings;

    public SpotifyService(HttpClient httpClient, IOptions<ServiceSettings> options)
    {
      this.httpClient = httpClient;
      settings = options.Value;
    }

    public record Token(string access_token);

    /// <summary> 
    ///   Authenticate in spotify usign oauth2 client credentials method.
    /// </summary>
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

    public record Playlists(PlaylistItems[] items);
    public record PlaylistItems(string name, string id);
    public record PlaylistsData(Playlists playlists);

    /// <summary> Get playlists based in a music genre </summary>
    /// <param name="genre">A string for spotify Genre Id</param>
    /// <returns>
    ///   JSON object with name and Id of each playlist
    /// </returns>
    private async Task<PlaylistsData> GetPlaylistsByGenreAsync(string genre)
    {
      var playlists = await httpClient
        .GetFromJsonAsync<PlaylistsData>(
          $"{settings.SpotifyHost}/v1/browse/categories/{genre}/playlists?offset=0&limit=5"
        );

      return playlists;
    }

    public record TrackItems(Track track);
    public record Track(string name, string href, TrackArtists[] artists);
    public record TrackArtists(string name);
    public record TrackData(TrackItems[] items);

    /// <summary> Get tracks for a playlists </summary>
    /// <param name="playlistId">String spotify Playlist Id</param>
    /// <returns>
    ///   JSON object with name, href and artists name of each track
    /// </returns>
    private async Task<TrackData> GetTracksFromPlaylistAsync(string playlistId)
    {
      var tracks = await httpClient
        .GetFromJsonAsync<TrackData>(
          $"{settings.SpotifyHost}/v1/playlists/{playlistId}/tracks?limit=10"
        );

      return tracks;
    }

    /// <summary> 
    ///   Return a list of tracks of an especific genre
    /// </summary>
    /// <param name="genre">String GenreId</param>
    /// <returns>JSON object with tracks data</returns>
    public async Task<TrackData> GetTracksByGenreAsync(string genre)
    {
      var res = await GetPlaylistsByGenreAsync(genre);

      var playlist = res.playlists.items[0];

      var tracks = await GetTracksFromPlaylistAsync(playlist.id);

      return tracks;
    }
  }
}
