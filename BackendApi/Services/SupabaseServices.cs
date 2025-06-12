using Supabase;
using System;
using System.Threading.Tasks;

namespace BackendApi.Services
{
    public class SupabaseServices
    {
        private Supabase.Client? _client;
        private const string SupabaseUrl = "https://hnyufgmenktqtmvdwlco.supabase.co";  // Your Supabase URL
        private const string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhueXVmZ21lbmt0cXRtdmR3bGNvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxMTE1MDQsImV4cCI6MjA1OTY4NzUwNH0.hWkJrNIhhkPK-8oDTGxTS-lnwNu0_9D-tAuvV1YEMrE";  // Your Supabase key

        public async Task InitializeAsync()
        {
            try
            {
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = true
                };

                // Initialize Supabase client
                _client = new Supabase.Client(SupabaseUrl, SupabaseKey, options);
                await _client.InitializeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Supabase client: {ex.Message}");
                throw;  // Optionally re-throw the exception to let the calling code handle it
            }
        }

        public Supabase.Client Client
        {
            get
            {
                if (_client == null)
                    throw new InvalidOperationException("Supabase client not initialized.");
                return _client;
            }
        }
    }
}
