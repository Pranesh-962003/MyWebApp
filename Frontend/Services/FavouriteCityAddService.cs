using System.Net.Http.Headers;

namespace Frontend.Services
{
    public class FavouriteCityAddService
    {
        private readonly HttpClient _httpClient;

        public FavouriteCityAddService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AddFavouriteCity(string city, string token)
        {



            var request = new FavoriteCityRequest
            {
                CityName = city
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responce = await _httpClient.PostAsJsonAsync($"{ApiConstants.BaseUrl}/favoriteCity/add", request);
            
            var message = await responce.Content.ReadAsStringAsync();
            return message;
        }
    }


    public class FavoriteCityRequest
    {
        public string CityName { get; set; } = string.Empty;
    }
}
