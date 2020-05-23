using System.ComponentModel.DataAnnotations;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantCounters
    {
        [Key]
        public int RestaurantId { get; set; }
        public string RestaurantUniqueId { get; set; }
        public int TotalViews { get; set; }
        public int TotalReservations { get; set; }
        public int TotalSuccessReservations { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }

        public virtual Restaurants Restaurant { get; set; }
    }
}
