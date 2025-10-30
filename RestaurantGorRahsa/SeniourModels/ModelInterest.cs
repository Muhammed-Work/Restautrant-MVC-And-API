using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.SeniourModels
{
    public class ModelInterest
    {
    
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Type ID is required.")]
        public string CustomerID { get; set; }

        public ModelCustomer Customer { get; set; }
    
    }

}


