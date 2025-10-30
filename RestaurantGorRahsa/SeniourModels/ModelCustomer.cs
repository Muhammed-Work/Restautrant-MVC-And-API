using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.SeniourModels
{
    public class ModelCustomer : ApplicationUser
    {
        public ModelCustomer() { 
        LstInterest=new HashSet<ModelInterest>();
        }

        public ICollection<ModelInterest> LstInterest { get; set; }

        
        
    }

}
