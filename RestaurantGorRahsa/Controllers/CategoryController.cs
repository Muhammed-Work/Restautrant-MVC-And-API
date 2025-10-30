using Microsoft.AspNetCore.Mvc;
using RestaurantGorRahsa.Services;
using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ITypeServices _typeServices;
        public CategoryController(ITypeServices typeServices)
        {
            _typeServices = typeServices;
        }

        public IActionResult CategoriesList()
        {
            var lstCategory = _typeServices.GetAllType();
            return View(lstCategory);
        }
        [HttpGet]
        public IActionResult CategoriesCreate(int ?id )
        {
           var model= new Models.ModelType();
            if (id != null)
            {
                model = _typeServices.GetModelType(Convert.ToInt32(id));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoriesCreate(Models.ModelType model)
        {
            if (ModelState.IsValid)
            {
                _typeServices.Save(model);
                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction("CategoriesList");
            }
            return View(model);
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            _typeServices.Delete(id);
            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction("CategoriesList");
        }
    }
}
