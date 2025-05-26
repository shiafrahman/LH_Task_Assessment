using Assessment_Backend.Dtos;
using Assessment_Backend.Models;
using Assessment_Backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartRepository.GetAllAsync();
            return Ok(carts);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Cart cart)
        {
            var product = await _productRepository.GetByIdAsync(cart.ProductId);
            if (product == null)
                return BadRequest("Invalid Product ID");

            await _cartRepository.AddAsync(cart);
            return Ok(cart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cart updatedCart)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            cart.ProductId = updatedCart.ProductId;
            cart.Quantity = updatedCart.Quantity;

            await _cartRepository.UpdateAsync(cart);
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            await _cartRepository.DeleteAsync(id);
            return NoContent();
        }


        [HttpPost("items")]
        public async Task<IActionResult> GetCartItems([FromBody] List<CartItemDto> items)
        {
            var result = new List<CartResponseDto>();

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                result.Add(new CartResponseDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImagePath = product.ImagePath,
                    Price = product.Price,
                    DiscountPercentage = product.DiscountPercentage,
                    DiscountStartDate = product.DiscountStartDate,
                    DiscountEndDate = product.DiscountEndDate,
                    Quantity = item.Quantity
                });
            }

            return Ok(result);
        }
    }
}
