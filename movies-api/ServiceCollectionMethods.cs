using movies_api.Interfaces.Services;
using movies_api.Services;

namespace movies_api
{
    public static class ServiceCollectionMethods
    {
        public static void AddApplicationServices(this IServiceCollection services) {
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddScoped<IMovieService, MovieService>();
        }
    }
}
