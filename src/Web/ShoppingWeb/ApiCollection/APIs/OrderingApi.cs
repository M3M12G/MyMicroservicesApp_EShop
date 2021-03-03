using Newtonsoft.Json;
using ShoppingWeb.API.Infrastructure;
using ShoppingWeb.ApiCollection.Infrastructure;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.ApiCollection.Settings;
using ShoppingWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiCollection.APIs
{
    public class OrderingApi: BaseHttpClientWithFactory, IOrderingApi
    {
        private IApiSettings _settings;
        public OrderingApi(IHttpClientFactory factory, IApiSettings settings) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.SetPath(_settings.OrderingPath);
        }

        public async Task Checkout(Order order)
        {
            _builder.SetPath(_settings.BasketPath).AddToPath("/Checkout");
            using var message = _builder.Content(new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"))
            .HttpMethod(HttpMethod.Post)
            .GetHttpMessage();
            await GetResponseStringAsync(message);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            using var message = _builder.AddQueryString("username", username)
            .HttpMethod(HttpMethod.Get)
            .GetHttpMessage();
            return await SendRequestAsync<IEnumerable<Order>>(message);
        }
    }
}
