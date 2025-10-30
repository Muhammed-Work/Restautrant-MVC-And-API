using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Services
{
    public interface IMealServices
    {
        public List<Models.ModelMeal> GetAllMeals();

        public Models.ModelMeal GetMealById(int id);

        public bool DeleteMeal(int id);

        public bool SaveMeal(ModelMeal meal);

        }

}
