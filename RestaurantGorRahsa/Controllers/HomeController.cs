using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantGorRahsa.Models;
using RestaurantGorRahsa.Services;
using RestaurantGorRahsa.viewModels;

namespace RestaurantGorRahsa.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMealServices _mealServices;
        private readonly IOrderServices _orderServices;

        public HomeController(IMealServices mealServices, IOrderServices orderServices) { 
            _orderServices= orderServices;
            _mealServices = mealServices; 
        }
        [HttpGet]
   //     [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewCart(string userId)
        {
            var cart =await _orderServices.GetCartByUser(userId);
                   if (cart == null)
            {
                return View(new List<ModelItem>()); // Return an empty cart view if not found
            }
            return View(cart.lstItems); // Pass the list of cart items to the view
        }

  //      [Authorize(Roles = "Customer")]
        public IActionResult Index()
        {
            try
            {
                var lstMeal=_mealServices.GetAllMeals();
                return View(lstMeal);
            }
            catch(Exception ex)  
            {
                return View(new List<ModelMeal>());
            }
            
        }

        [HttpGet]
     //   [Authorize(Roles = "Customer")]

        public IActionResult Detail(int id)
        {
            var meal=new mealOrderVM();
            if (id != 0)
            {
               var modelmeal = _mealServices.GetMealById(id);
                if (modelmeal != null) {
                meal.Name = modelmeal.Name;
                meal.MealId = id;
                meal.OfferedNumber = modelmeal.OfferedNumber;
                 meal.price = modelmeal.price;
                  meal.ImageName = modelmeal.ImageName;
                }
            }
            return View(meal); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [Authorize(Roles = "Customer")]

        public async Task <IActionResult> MakeOrder(mealOrderVM mealOrder)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Detail", new { id = mealOrder.MealId });
            }

            // التحقق من UserId وطلب الخدمة
            try
            {
                if (string.IsNullOrEmpty(mealOrder.UserID))
                {
                    throw new Exception("User ID is required.");
                }

                
              await  _orderServices.AddItemToCart(mealOrder, mealOrder.UserID);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // معالجة الخطأ وعرض رسالة للمستخدم إذا لزم الأمر
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return RedirectToAction("Detail", new { id = mealOrder.MealId });
            }
        }

//        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Cancelitem(int itemID)
        {
            try
            {
                await _orderServices.CancelItemFromCart(itemID);
                TempData["Success"] = "Item successfully removed from your cart.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("ViewCart");
        }





    }
}
