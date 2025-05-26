using Assessment_Frontend.Models;
using Assessment_Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Assessment_Frontend.Controllers
{
    public class CartController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string SessionKey = "CartItems";
        public CartController(IApiService apiService, IHttpClientFactory httpClientFactory)
        {
            _apiService = apiService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Cart()
        {
            var cartItems = await _apiService.GetCart();
            return View(cartItems);
        }

        private List<CartItem> GetSessionCart()
        {
            var json = HttpContext.Session.GetString(SessionKey);
            return string.IsNullOrEmpty(json) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        private void SaveSessionCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(cart));
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null) item.Quantity++;
            else cart.Add(new CartItem { ProductId = productId, Quantity = 1 });
            SaveSessionCart(cart);
            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetSessionCart();
            if (!cart.Any()) return View(new List<CartResponse>());

            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/Cart/items", cart);
            var products = await response.Content.ReadFromJsonAsync<List<CartResponse>>();

            ViewBag.Total = products.Sum(p => p.TotalPrice);
            return View(products);
        }

        public IActionResult Remove(int productId)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null) cart.Remove(item);
            SaveSessionCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult GetCartCount()
        {
            var cart = GetSessionCart();
            return Json(new { count = cart.Sum(item => item.Quantity) });
        }
    }
}
