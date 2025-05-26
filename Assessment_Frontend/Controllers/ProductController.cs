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

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            await _apiService.AddToCart(productId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> Add()
        {
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

            

            // Replace this with your actual API/service call
            await _apiService.AddProductAsync(product);

            return RedirectToAction("Index");
        }

        
    }
}
