using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendApi.Models
{
    public class MCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Email { get; set; } = "";
        public string CityName { get; set; } = "";
        public DateTime Date { get; set; }
        public string Summary { get; set; } = "";
        public double Temperature { get; set; }
    }
}
