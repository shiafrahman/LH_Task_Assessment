using Assessment_Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Assessment_Frontend.Models;

namespace Assessment_Frontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IApiService _apiService;
        private readonly HttpClient _httpClient;

        public ProductController(IApiService apiService, HttpClient httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index(string search = "", int page = 1, int pageSize = 10)
        {
            
            var productResponse = await _apiService.GetProducts(search, page, pageSize);

           
            var products = productResponse?.Products ?? new List<Product>();
            var totalPages = productResponse?.TotalPages ?? 0;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = search;
            ViewBag.PageSize = pageSize;
            ViewBag.CartCount = GetCartCount();

            return View(products);
        }

        public async Task<IActionResult> Add(string search = "", int page = 1, int pageSize = 10)
        {
            //var productResponse = await _apiService.GetProducts("", 1, 10);
            var productResponse = await _apiService.GetProducts(search, page, pageSize);
            var totalPages = productResponse?.TotalPages ?? 0;
            ViewBag.Products = productResponse?.Products ?? new List<Product>();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = search;
            ViewBag.PageSize = pageSize;
            ViewBag.CartCount = GetCartCount();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImages", fileName);
                product.Image = ImageFile;

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.ImagePath = "/ProductImages/" + fileName;
            }

            

            
            await _apiService.AddProductAsync(product);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var productResponse = await _apiService.GetProductById(id);
            if (productResponse == null)
                return NotFound();

            ViewBag.CartCount = GetCartCount();
            return View(productResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile ImageFile)
        {
            if (id != product.Id)
                return BadRequest();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductImages", fileName);
                product.Image = ImageFile;

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.ImagePath = "/ProductImages/" + fileName;
            }

            await _apiService.UpdateProductAsync(product);
            return RedirectToAction("Add");
        }


        private int GetCartCount()
        {
            var json = HttpContext.Session.GetString("CartItems");
            var cart = string.IsNullOrEmpty(json) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json);
            return cart?.Sum(x => x.Quantity) ?? 0;
        }


    }
}
