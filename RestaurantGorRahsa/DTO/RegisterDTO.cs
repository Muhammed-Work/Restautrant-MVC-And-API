using System.ComponentModel.DataAnnotations;

namespace RestaurantGorRahsa.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(256)]
        public  string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
