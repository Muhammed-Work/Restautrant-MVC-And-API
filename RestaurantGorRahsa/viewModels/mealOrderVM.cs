using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.viewModels
{
  
        public class mealOrderVM
        {
        public int MealId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Qty { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public float price { get; set; }

        
        public string ImageName { get; set; }
        public int OfferedNumber { get; set; }

        [Required(ErrorMessage = "UserID is required.")]
        public string UserID { get; set; }
    }
}

