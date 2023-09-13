using Microsoft.Extensions.Caching.Memory;
using movies_api.Constants;
using movies_api.Interfaces.Services;
using movies_api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace movies_api.Services
{
    public class MovieService: IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public MovieService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = memoryCache;
        }

        public async Task<List<Movie>> GetAll()
        {
            try
            {
                string jsonUrl = MovieConstants.MovieDataUrl;

                HttpResponseMessage response = await _httpClient.GetAsync(jsonUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{ErrorMessages.HttpRequestError} Status code: {response.StatusCode}");
                }

                string jsonData = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(jsonData))
                {
                    return new List<Movie>();
                }

                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                var movies = jsonObject["movies"].ToObject<List<Movie>>();
                movies?.ForEach(movie => movie.Cover = movie.Cover.Replace("htps://", "https://"));
                await CacheMovieImages(movies);
                return movies;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"{ErrorMessages.HttpRequestError}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"{ErrorMessages.JsonDeserializationError}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ErrorMessages.GeneralError}: {ex.Message}", ex);
            }
        }

        public async Task CacheMovieImages(List<Movie>? movies)
        {
            if (movies == null || movies.Count == 0 || _cache == null || _httpClient == null)
            {
                return;
            }

            foreach (var movie in movies)
            {
                if (!string.IsNullOrEmpty(movie.Cover))
                {
                    string imageUrl = movie.Cover;

                    // Check if the image is already in the cache
                    if (!_cache.TryGetValue(imageUrl, out _))
                    {
                        try
                        {
                            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                _cache.Set(imageUrl, imageBytes, TimeSpan.FromMinutes(30)); // Cache the image data for 30 minutes
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            throw new Exception($"{ErrorMessages.HttpRequestError}: {ex.Message}", ex);
                        }
                    }
                }
            }
        }

    }
}
