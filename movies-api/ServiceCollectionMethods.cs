using movies_api.Services;

namespace movies_api
{
    public static class ServiceCollectionMethods
    {

        public static void AddApiServices(this ServiceCollection services) { 
            services.AddScoped<MovieService, MovieService>();
            }
    }
}
