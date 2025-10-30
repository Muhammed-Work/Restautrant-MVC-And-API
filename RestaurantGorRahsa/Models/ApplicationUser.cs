using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Carts = new HashSet<ModelCart>();
        }
        [Required]
        [StringLength(256)]
        public override string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        // Other properties...
        public ICollection<ModelCart> Carts { get; set; }  // Ensure this collection exists    }

        public bool IsApproved { get; set; }

    }
}