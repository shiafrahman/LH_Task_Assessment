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

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
                item.Quantity++;
            else
                cart.Add(new CartItem { ProductId = productId, Quantity = 1 });

            SaveSessionCart(cart);


            await _apiService.AddToCart(productId);

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Index()
        {
            var cart = GetSessionCart();
            List<CartResponse> cartItems;

            try
            {
                
                if (!cart.Any())
                {
                    var backendCart = await _apiService.GetCart();
                    
                    cart = backendCart.Select(c => new CartItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity
                    }).ToList();
                    SaveSessionCart(cart); 
                }

                
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.PostAsJsonAsync("api/Cart/items", cart);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<CartResponseWrapper>();

                cartItems = result?.Items ?? new List<CartResponse>();
                ViewBag.Total = result?.Items.Sum(p => p.TotalPrice) ?? 0;
               
            }
            catch (HttpRequestException)
            {
                
                ViewBag.ErrorMessage = "Unable to fetch cart items from the server. Displaying local cart data.";
                cartItems = new List<CartResponse>();
                ViewBag.Total = 0;
                
            }

            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var cart = GetSessionCart();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveSessionCart(cart);
                await _apiService.DeleteCart(productId);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Item not found" });
        }



        public async Task<IActionResult> Cart()
        {
            return RedirectToAction("Index"); 
        }


        public IActionResult GetCartTotal()
        {
            var cart = GetSessionCart();
            var total = cart.Sum(item => item.Quantity * item.DiscountedPrice);
            return Json(total.ToString("C"));
        }

        public IActionResult GetCartCount()
        {
            var json = HttpContext.Session.GetString("CartItems");
            var cart = string.IsNullOrEmpty(json)
                ? new List<CartResponse>()
                : JsonSerializer.Deserialize<List<CartResponse>>(json);

            return Json(new { count = cart.Sum(x => x.Quantity) });
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
    }
}
