using BackendApi.Models;
using MongoDB.Driver;
namespace BackendApi.Services
{
    public class MdbSettingsService
    {
        private readonly IMongoCollection<MCollection> _collection;

        public MdbSettingsService(IConfiguration config) {
            var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings?.ConnectionString);
            var database = client.GetDatabase(settings?.Database);
            _collection = database.GetCollection<MCollection>(settings!.Collection);
        }

        public async Task SaveForecastAsync(List<MCollection> entries)
        {
            await _collection.InsertManyAsync(entries);
        }

        public async Task<List<MCollection>> GetForecastByEmailAsync(string email)
        {
            return await _collection.Find(e => e.Email == email).ToListAsync();
        }
    }
}
