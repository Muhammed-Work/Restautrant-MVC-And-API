using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantGorRahsa.Models;
using RestaurantGorRahsa.viewModels;

namespace RestaurantGorRahsa.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly RMScontext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderServices(RMScontext context,
             UserManager<ApplicationUser> userManager) { _context = context; _userManager = userManager; }

        public async Task AddItemToCart(viewModels.mealOrderVM mealOrderVM, string userId)
        {
            try
            {
                // 1. Fetch the meal from the database
                var meal = await _context.TbMeal.FirstOrDefaultAsync(e => e.ID == mealOrderVM.MealId);
                if (meal == null)
                {
                    Console.WriteLine("Meal not found.");
                    return;
                }

                // 2. Check if the requested quantity is available
                if (meal.OfferedNumber < mealOrderVM.Qty)
                {
                    Console.WriteLine("Not enough stock available for the meal.");
                    return;
                }

                // 3. Decrease the available quantity
                meal.OfferedNumber -= mealOrderVM.Qty;
                _context.Entry(meal).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // 4. Retrieve the user's cart or create a new one
                var cart = await _context.TbCart
                    .Include(c => c.lstItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    // Retrieve the user by ID
                    var user = await _userManager.FindByEmailAsync(userId);
                    if (user == null)
                    {
                        Console.WriteLine("User not found.");
                        return;
                    }

                    cart = new ModelCart
                    {
                        User = user,
                        UserId = user.Id,
                        lstItems = new List<ModelItem>()
                    };
                    _context.TbCart.Add(cart);
                    await _context.SaveChangesAsync();
                }

                // 5. Check if the meal already exists in the cart
                var itemExisting = cart.lstItems.FirstOrDefault(i => i.MealID == mealOrderVM.MealId);
                if (itemExisting != null)
                {
                    // Update quantity if the meal exists
                    itemExisting.Quantity += mealOrderVM.Qty;
                    _context.Entry(itemExisting).State = EntityState.Modified;
                }
                else
                {
                    // Add a new item to the cart
                    var newItem = new ModelItem
                    {
                        MealID = meal.ID,
                        meal = meal,
                        Quantity = mealOrderVM.Qty,
                        Price = meal.price,
                        Name = meal.Name,
                        CartID = cart.Id,
                        cart = cart
                    };
                    cart.lstItems.Add(newItem);
                }

                // 6. Save the cart to the database
                _context.Entry(cart).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task<ModelCart> GetCartByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            // Retrieve the user by ID
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return null;
            }

            // Retrieve the cart with items for the specified user
            var cart = await _context.TbCart
                .Include(c => c.lstItems)
                .ThenInclude(i => i.meal) // Include meal details for each item
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

          return cart;
        }


        public async Task CancelItemFromCart(int itemId)
        {
            try
            {
                // Retrieve the item from the cart
                var item = await _context.TbItem.Include(i => i.meal).FirstOrDefaultAsync(i => i.Id == itemId);
                if (item == null)
                {
                    Console.WriteLine("Cart item not found.");
                    return;
                }

                // Restore the meal's stock
                var meal = await _context.TbMeal.FirstOrDefaultAsync(e => e.ID == item.meal.ID);
                if (meal != null)
                {
                    meal.OfferedNumber += item.Quantity; // Increment the stock by the canceled quantity
                    _context.Entry(meal).State = EntityState.Modified;
                }

                // Remove the item from the cart
                _context.TbItem.Remove(item);
                await _context.SaveChangesAsync();

                Console.WriteLine("Item successfully removed from the cart.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


    }
}


//try
//{
//    var meal = _context.TbMeal.FirstOrDefault(e => e.ID == mealOrderVM.MealId);
//    meal.OfferedNumber -= mealOrderVM.Qty;

//    var item = new ModelItem();
//    item.meal = meal;
//    item.MealID = meal.ID;
//    item.Price = meal.price;
//    item.Quantity = mealOrderVM.Qty;
//    _context.TbItem.Add(item);
//    _context.Entry(meal).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
//    _context.SaveChanges();
//}
//catch (Exception ex)
//{

//}