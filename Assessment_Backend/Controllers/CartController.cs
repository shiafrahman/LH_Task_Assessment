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

        // GET: api/Cart
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartRepository.GetAllAsync();
            var cartResponse = carts.Select(c => new CartResponseDto
            {
                ProductId = c.ProductId,
                Name = c.Product.Name,
                ImagePath = c.Product.ImagePath,
                Price = c.Product.Price,
                DiscountPercentage = c.Product.DiscountPercentage,
                DiscountStartDate = c.Product.DiscountStartDate,
                DiscountEndDate = c.Product.DiscountEndDate,
                Quantity = c.Quantity
            }).ToList();
            return Ok(new { Items = cartResponse, TotalCount = cartResponse.Sum(c => c.Quantity) });
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            var cartResponse = new CartResponseDto
            {
                ProductId = cart.ProductId,
                Name = cart.Product.Name,
                ImagePath = cart.Product.ImagePath,
                Price = cart.Product.Price,
                DiscountPercentage = cart.Product.DiscountPercentage,
                DiscountStartDate = cart.Product.DiscountStartDate,
                DiscountEndDate = cart.Product.DiscountEndDate,
                Quantity = cart.Quantity
            };
            return Ok(cartResponse);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartItemDto cartItem)
        {
            if (cartItem.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null)
                return BadRequest("Invalid Product ID");

            
            var existingCart = await _cartRepository.GetByProductIdAsync(cartItem.ProductId);
            if (existingCart != null)
            {
                existingCart.Quantity += cartItem.Quantity;
                await _cartRepository.UpdateAsync(existingCart);
                return Ok(new CartResponseDto
                {
                    ProductId = existingCart.ProductId,
                    Name = product.Name,
                    ImagePath = product.ImagePath,
                    Price = product.Price,
                    DiscountPercentage = product.DiscountPercentage,
                    DiscountStartDate = product.DiscountStartDate,
                    DiscountEndDate = product.DiscountEndDate,
                    Quantity = existingCart.Quantity
                });
            }

            var cart = new Cart
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };

            await _cartRepository.AddAsync(cart);
            var response = new CartResponseDto
            {
                ProductId = cart.ProductId,
                Name = product.Name,
                ImagePath = product.ImagePath,
                Price = product.Price,
                DiscountPercentage = product.DiscountPercentage,
                DiscountStartDate = product.DiscountStartDate,
                DiscountEndDate = product.DiscountEndDate,
                Quantity = cart.Quantity
            };
            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, response);
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
                if (product == null)
                    continue;

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

            return Ok(new { Items = result, TotalCount = result.Sum(c => c.Quantity) });
        }
    }
}
