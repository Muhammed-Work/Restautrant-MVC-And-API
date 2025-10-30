using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.viewModels
{
    public class LoginModel
    {
        [Required]
        [StringLength(256)]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
