using Supabase.Postgrest.Attributes;

namespace BackendApi.Models
{
    [Table("tracked_forecasts")]
    public class TrackedForecast : BaseModel
    {
        [PrimaryKey("id", false)]
        public new int Id { get; set; }

        [Column("city")]
        public string City { get; set; } = string.Empty; // Default empty

        [Column("weather_main")]
        public string WeatherMain { get; set; } = string.Empty; // Default empty

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; // Default to now

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to now
    }

    public class BaseModel : Supabase.Postgrest.Models.BaseModel
    {
        // You can add any shared properties here if necessary.
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
