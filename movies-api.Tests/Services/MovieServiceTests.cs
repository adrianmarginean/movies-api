using Microsoft.Extensions.Caching.Memory;
using Moq;
using movies_api.Models;
using movies_api.Services;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using Assert = NUnit.Framework.Assert;

namespace movies_api.Tests.Services
{
        [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
        public class MovieServiceTests
        {

        private MovieService _movieService;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private MemoryCache _memoryCache;


        public MovieServiceTests()
        {

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            // Create and configure a mock HttpClient
            var httpClient = new HttpClient(new MockHttpMessageHandler(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{ ""movies"": [{ ""Name"": ""Movie 1"", ""Description"": ""Description 1"", ""Cover"": ""https://example.com/image1.jpg"" }] }"),
                }
            ));

            // Configure the mock to return the HttpClient when CreateClient is called
            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _memoryCache = new MemoryCache(new MemoryCacheOptions());


            _movieService = new MovieService(_httpClientFactoryMock.Object, _memoryCache);

        }

            [Test]
            public async Task GetAll_ValidData_ReturnsMovies()
            {
                // Arrange

                // Act
                var movies = await _movieService.GetAll();

                // Assert
                Assert.IsNotNull(movies);
                Assert.That(movies.Count, Is.EqualTo(1));
                Assert.That(movies[0].Name, Is.EqualTo("Movie 1"));
            }

        [Test]
        public async Task CacheMovieImages_ValidData_CachesImages()
        {
            // Arrange
            var movie = new Movie
            {
                Name = "Movie 1",
                Description = "Description 1",
                Cover = "https://example.com/image1.jpg",
            };

            // Act
            await _movieService.CacheMovieImages(new List<Movie> { movie });

            // Assert
            if (_memoryCache.TryGetValue(movie.Cover, out _))
            {
                Assert.Pass(); // Cache key exists, test passed
            }
            else
            {
                Assert.Fail("Cache key not found"); // Cache key not found, test failed
            }
        }
    }

        // Helper class to mock HttpClient responses
        public class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public MockHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                tcs.SetResult(_response);
                return tcs.Task;
            }
        }
    }

