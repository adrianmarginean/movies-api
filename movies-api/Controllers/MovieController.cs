using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movies_api.Models;
using movies_api.Services;

namespace movies_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
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
