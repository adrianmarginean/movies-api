using Microsoft.AspNetCore.Mvc;
using movies_api.Interfaces.Services;

namespace movies_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movies = await _movieService.GetAll();
                return Ok(movies);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
