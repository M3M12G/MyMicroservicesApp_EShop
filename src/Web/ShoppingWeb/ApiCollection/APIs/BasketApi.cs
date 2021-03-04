using Newtonsoft.Json;
using ShoppingWeb.ApiCollection.Infrastructure;
using ShoppingWeb.ApiCollection.Interfaces;
using ShoppingWeb.ApiCollection.Settings;
using ShoppingWeb.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.ApiCollection.APIs
{
    public class BasketApi : BaseHttpClientWithFactory, IBasketApi
    {
        private IApiSettings _settings;

        public BasketApi(IApiSettings settings, IHttpClientFactory factory) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.AddToPath(_settings.BasketPath);
        }

        //Cart ~ BasketCart, CartItem ~ BasketCartItem

        public async Task<Cart> AddItem(string username, CartItem item)
        {
            var message = _builder
        .HttpMethod(HttpMethod.Post).AddQueryString("username", username).
        Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
        .GetHttpMessage();
            return await SendRequestAsync<Cart>(message);
        }

        public async Task<bool> DeleteCart(string username)
        {
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath))
            {
                var message = _builder.AddToPath(username)
            .HttpMethod(HttpMethod.Delete)
            .GetHttpMessage();
                var response = await GetResponseStringAsync(message);
                return response != null;
            }
        }

        public async Task<bool> DeleteItem(string username, CartItem item)
        {
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath))
            {
                using var message = _builder.
                    AddQueryString("username", username).
                Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
                .HttpMethod(HttpMethod.Delete)
                .GetHttpMessage();
                var response = await GetResponseStringAsync(message);
                return response != null;
            }
        }

        public async Task<Cart> GetCart(string username)
        {
            using var message = _builder.AddQueryString("username", username)
             .HttpMethod(HttpMethod.Get)
             .GetHttpMessage();
            return await SendRequestAsync<Cart>(message);
        }
    }
}
