using movies_api.Models;

namespace movies_api.Interfaces.Services
{
    public interface IMovieService
    {
        public Task<List<Movie>> GetAll();
    }
}
