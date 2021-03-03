using ShoppingWeb.Models;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiCollection.Interfaces
{
    public interface IBasketApi
    {
        Task<Cart> GetCart(string username);
        Task<bool> DeleteCart(string username);
        Task<Cart> AddItem(string username, CartItem item);
        Task<bool> DeleteItem(string username, CartItem item);
    }
}
