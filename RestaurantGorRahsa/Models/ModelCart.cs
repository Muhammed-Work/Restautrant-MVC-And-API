using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantGorRahsa.Models
{
    public class ModelCart
    {
        public ModelCart()
        { 
        lstItems = new HashSet<ModelItem>();
        }
    
        [Key]
        public int Id { get; set; }

        public ICollection<ModelItem> lstItems { get; set; }


        // Navigation property for one-to-one relationship
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }

}
