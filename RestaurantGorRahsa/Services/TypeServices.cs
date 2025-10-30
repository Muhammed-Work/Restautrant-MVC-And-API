using Microsoft.DotNet.Scaffolding.Shared.Project;
using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Services
{
    public class TypeServices : ITypeServices
    {

        private RMScontext _context;
        public TypeServices(RMScontext context)
        {
            _context = context;
        }

        public bool Delete(int id)
        {
            try
            {
                // Retrieve the category by ID
                var category = _context.TbType.FirstOrDefault(e => e.Id == id);
                if (category == null)
                {
                    return false; // Category not found
                }

                // Retrieve all related meals
                var relatedMeals = _context.TbMeal.Where(e => e.type == category).ToList();

                // Remove all related meals if any exist
                if (relatedMeals.Any())
                {
                    _context.TbMeal.RemoveRange(relatedMeals);
                }

                // Remove the category
                _context.TbType.Remove(category);

                // Save all changes
                _context.SaveChanges();

                return true; // Successful deletion
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false; // Indicate failure
            }
        }


        public List<Models.ModelType> GetAllType()
        {
            try {
            var lstCategory=_context.TbType.ToList();
            return lstCategory;
            }
            catch (Exception ex) { 
            return new List<Models.ModelType>();
            }
        }

        public Models.ModelType GetModelType(int id)
        {
            try {
            var Category= _context.TbType.FirstOrDefault(e=>e.Id==id);
                return Category;
            }
            catch (Exception ex) { 
            return new Models.ModelType();
            }
        }





        public bool Save(Models.ModelType category)
        {
            try
            {
                if (category == null)
                {
                    return false; // Null entity passed
                }

                if (category.Id == 0) // New entity
                {
                    _context.Add(category);
                }
                else // Existing entity
                {
                    var existingCat = _context.TbType.FirstOrDefault(c => c.Id == category.Id);
                    if (existingCat == null)
                    {
                        return false; // Entity not found
                    }
                    
                    existingCat.Name = category.Name;
            
                    // Add other fields if ModelType has more properties
                    _context.Entry(existingCat).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                // Save changes (single call for both add and update)
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error saving category: {ex.Message}");
                return false; // Indicate failure
            }
        }



    }
}
