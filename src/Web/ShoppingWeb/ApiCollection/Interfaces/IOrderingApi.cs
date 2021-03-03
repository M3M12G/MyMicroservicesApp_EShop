using ShoppingWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiCollection.Interfaces
{
    public interface IOrderingApi
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
        Task Checkout(Order order);
    }
}
