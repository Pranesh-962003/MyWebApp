
namespace Frontend.Models
{
    public class ForcastEntity
    {
         // MongoDB _id
        public string Email { get; set; } = "";
        public string CityName { get; set; } = "";
        public DateTime Date { get; set; }
        public string Summary { get; set; } = "";
        public double Temperature { get; set; }
    }
}
