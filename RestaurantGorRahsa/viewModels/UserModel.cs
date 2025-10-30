using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.viewModels
{
    public class UserModel
    {
        [Required]
        [StringLength(256)]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        [StringLength(256)]

        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
