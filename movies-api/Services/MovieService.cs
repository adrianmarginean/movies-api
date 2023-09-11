using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using movies_api.Constants;
using movies_api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace movies_api.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<Movie>> GetAll()
        {
            try
            {
                string jsonUrl = "https://raw.githubusercontent.com/cosmin19/movie-database/master/data.json";

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

    }
}
