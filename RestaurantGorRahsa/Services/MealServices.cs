using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Services
{
    public class MealServices : IMealServices
    {
        private readonly RMScontext _context;
        public MealServices(RMScontext context) { _context = context; }

        public List<ModelMeal> GetAllMeals()
        {
            try
            {
                var lstMeal = _context.TbMeal.ToList();
                return lstMeal;
            }
            catch (Exception ex)
            {
                return new List<ModelMeal>();
            }
        }

        public Models.ModelMeal GetMealById(int id)
        {
            try
            {
                var meal = _context.TbMeal.FirstOrDefault(e => e.ID == id);
                return meal;
            }
            catch (Exception ex)
            {
                return new ModelMeal();
            }
        }

        public bool DeleteMeal(int id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }
                var meal = _context.TbMeal.FirstOrDefault(_ => _.ID == id);
                if (meal != null)
                {
                    _context.TbMeal.Remove(meal);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }

        }

        public bool SaveMeal(ModelMeal meal)
        {
            try
            {
                if(meal == null) { return false; }
                var MealExisting=_context.TbMeal.FirstOrDefault(e=>e.ID == meal.ID);
                if (MealExisting != null)
                {
                    MealExisting.Name = meal.Name;
                    MealExisting.price = meal.price;
                    MealExisting.OfferedNumber = meal.OfferedNumber;
                    MealExisting.ImageName = meal.ImageName;
                    MealExisting.type = _context.TbType.FirstOrDefault(e => e.Id == meal.TypeID);
                    MealExisting.TypeID = meal.TypeID;
                    
                    _context.Entry(MealExisting).State = Microsoft.EntityFrameworkCore.EntityState.Modified;


                }
                else {
                meal.type = _context.TbType.FirstOrDefault(e => e.Id == meal.TypeID);
                 

                    _context.Add(meal);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving meal: {ex.Message}");
                return false;
            }

        }



    }
}
