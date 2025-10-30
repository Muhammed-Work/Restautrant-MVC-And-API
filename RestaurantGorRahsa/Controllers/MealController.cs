using Microsoft.AspNetCore.Mvc;
using RestaurantGorRahsa.Helper;
using RestaurantGorRahsa.Models;
using RestaurantGorRahsa.Services;

namespace RestaurantGorRahsa.Controllers
{
    public class MealController : Controller
    {
        private readonly IMealServices _mealServices;
        private readonly ITypeServices _typeServices;
        public MealController(IMealServices mealServices, ITypeServices typeServices)
        { 
            _mealServices = mealServices; 
            _typeServices = typeServices;
        }

        [HttpGet]
        public IActionResult MealsList()
        {
            try
            {
                var lstMeal=_mealServices.GetAllMeals();
                return View(lstMeal);
            }
            catch (Exception ex)
            {
                return View(new List<ModelMeal>());
            }

        }
      
        [HttpGet]        
        public IActionResult CreateEdite(int ?id)
        {
            ViewBag.Types = _typeServices.GetAllType();
            var meal = new ModelMeal();
            if (id !=null) {
                meal = _mealServices.GetMealById(Convert.ToInt32(id));
                if (meal != null)
                {
                    meal.TypeID = meal.type?.Id ?? 0;  // Set the TypeID in case it's needed for the form
                }
            }
            return View(meal);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdite(ModelMeal meal,List<IFormFile> files)
        {
          
                
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Log or display the error message
                    Console.WriteLine(error.ErrorMessage);
                }
                ViewBag.Types = _typeServices.GetAllType();  // Re-populate types in case of errors
                return View("CreateEdite", meal);
            }
            meal.ImageName = await toolsHelper.UploadImage(files, "Images");
            _mealServices.SaveMeal(meal);
            return RedirectToAction("MealsList");
        
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _mealServices.DeleteMeal(id);
            TempData["SuccessMessage"] = "Meal deleted successfully!";
            return RedirectToAction("MealsList");
        }



    }
}
