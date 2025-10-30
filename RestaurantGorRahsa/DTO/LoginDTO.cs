using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.DTO
{
    public class LoginDTO
    {
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        
        [Required]
        public string  Password { get; set; }

    }

}
