using BackendApi.Models;
using Supabase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services
{
    public class FavoriteCityService
    {
        private readonly SupabaseServices _supabaseServices;

        public FavoriteCityService(SupabaseServices supabaseServices)
        {
            _supabaseServices = supabaseServices;
        }

        // Add a favorite city
        public async Task<bool> AddFavoriteCityAsync(string userId, string cityName)
        {
            var client = _supabaseServices.Client;

           
            var session = client.Auth.CurrentSession;
            if (session == null || session.User == null)
            {
                Console.WriteLine("User is not authenticated.");
                return false;  
            }

            
            string? authenticatedUserId = session.User?.Id;

            if (authenticatedUserId == null)
            {
                Console.WriteLine("Authenticated user ID is null.");
                return false; 
            }

            
            if (authenticatedUserId != userId)
            {
                Console.WriteLine("The provided userId does not match the authenticated user's ID.");
                return false;  // Return false if userIds do not match
            }

            
            if (!Guid.TryParse(authenticatedUserId, out Guid userGuid))
            {
                Console.WriteLine("Invalid userId format.");
                return false;
            }

           
            var city = new FavouriteCity
            {
                user_id = userGuid,  
                city_name = cityName
            };

            try
            {
                
                var response = await client
                    .From<FavouriteCity>()
                    .Insert(city);

                
                Console.WriteLine("Full Response: " + response.ToString());

                // Check if Models are returned
                if (response.Models == null || response.Models.Count == 0)
                {
                    Console.WriteLine("Error: Models is null or empty.");
                    return false;
                }

                // Log the successful insert
                Console.WriteLine($"Successfully added city: {cityName}");
                return true;
            }
            catch (Exception ex)
            {
                // Catch any exceptions and print the message
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }












        public async Task<List<AddFavoriteCityDto>> GetFavoriteCitiesAsync(string userId)
        {
            var client = _supabaseServices.Client;

            // Try parsing the userId into a Guid
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                Console.WriteLine("Invalid userId format.");
                return new List<AddFavoriteCityDto>();  // Return an empty list if userId is invalid
            }

            Console.WriteLine(userGuid);

            try
            {
                // Query to retrieve favorite cities for the given user
                var response = await client
                    .From<FavouriteCity>()  // Ensure this is the model used for fetching
                    .Where(city => city.user_id == userGuid) // Filter by user_id
                    .Select("city_name") // Select only the 'city_name' column
                    .Get(); // Execute the query to retrieve results

                // Check if the response contains valid models
                if (response.Models == null || response.Models.Count == 0)
                {
                    Console.WriteLine($"No favorite cities found for user {userId}.");
                    return new List<AddFavoriteCityDto>(); // Return an empty list if no cities are found
                }

                // Convert the response to a list of FavouriteCityFetchDto
                var favoriteCities = response.Models.Select(city => new AddFavoriteCityDto
                {
                    city_name = city.city_name
                }).ToList();

                // Return the list of favorite cities
                return favoriteCities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<AddFavoriteCityDto>(); // Return an empty list in case of an error
            }
        }

    }


}

