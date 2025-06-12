using Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace BackendApi.Models
{

    public class FavouriteCity : BaseModel
    {
        [PrimaryKey]
         // Use the type that matches your DB column (likely 'int' or 'long')
        public Guid? user_id { get; set; }
        public string? city_name { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? Id { get; set; }
    }
}
