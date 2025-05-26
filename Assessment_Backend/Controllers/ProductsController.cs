using Assessment_Backend.Dtos;
using Assessment_Backend.Models;
using Assessment_Backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, int page = 1, int pageSize = 10)
        {
            var products = await _productRepository.GetAllAsync(search, page, pageSize);
            var totalPages = await _productRepository.GetTotalPages(search, pageSize);
            return Ok(new
            {
                products,
                totalPages
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductDto dto)
        {
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot/ProductImages");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, dto.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                dto.ImagePath = "/ProductImages/" + dto.Image.FileName;
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DiscountPercentage = dto.DiscountPercentage,
                DiscountStartDate = dto.DiscountStartDate,
                DiscountEndDate = dto.DiscountEndDate,
                ImagePath = dto.ImagePath
            };

            await _productRepository.AddAsync(product);
            return Ok(product);
        }
    }
}
