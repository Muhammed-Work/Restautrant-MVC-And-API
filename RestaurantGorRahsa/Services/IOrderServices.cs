using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Services
{
    public interface IOrderServices
    {
        public Task AddItemToCart(viewModels.mealOrderVM mealOrderVM, string userId);
        public Task<ModelCart> GetCartByUser(string userId);
        public Task CancelItemFromCart(int itemId);
    }
}
