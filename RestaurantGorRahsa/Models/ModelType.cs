
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

namespace RestaurantGorRahsa.Models
{
    public class ModelType
    {

        public ModelType() {
            lstMeal = new HashSet<ModelMeal>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [JsonIgnore] // تجاهل هذه الخاصية عند التسلسل إلى JSON

        public ICollection<ModelMeal> lstMeal { get; set; }

    }
}
