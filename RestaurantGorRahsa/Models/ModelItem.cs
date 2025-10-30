using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantGorRahsa.Models
{
    public class ModelItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int MealID { get; set; }
        [ForeignKey("MealID")]
        [ValidateNever]
        public ModelMeal meal { get; set; }

        [Required]
        public int CartID { get; set; }
        [ForeignKey("MealID")]
        [ValidateNever]
        public ModelCart cart { get; set; }

    }
}
