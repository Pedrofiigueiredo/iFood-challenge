using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tracker.Repositories;

namespace tracker.Controllers
{
  [ApiController]
  [Route("v1/")]
  public class TracksController : ControllerBase
  {
		private readonly TracksRepository _repository;
		public TracksController(TracksRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("{city}")]
		public async Task<dynamic> Get(string city)
		{
			try
			{
				var data = await _repository.GetTracksSuggestions(city);
			
				return data;
			}
			catch
			{
				return "invalid city";
			}
		}
  }
}
