using Microsoft.Extensions.Configuration;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Models;
using MovieSearch.Domain.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieSearch.BL.Impls.Services
{
    public class OmDbMovieService : IMovieService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public OmDbMovieService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<Movie> Search(string s)
        {
            Movie movie = null;

            string urlBase = configuration.GetSection("OmDb").GetValue<string>("Url");

            string url = $"{urlBase}&r=json&s={s}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("Accept", "application/json");

            var client = httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string strResponse = await response.Content.ReadAsStringAsync();

                var searchResponse = JsonConvert.DeserializeObject<MovieSearchResponse>(strResponse);

                if (searchResponse.Response)
                {
                    movie = searchResponse.Search.FirstOrDefault();
                }
            }

            return movie;
        }
    }
}
