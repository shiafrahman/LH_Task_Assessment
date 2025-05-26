using Assessment_Frontend.Models;
using System.Text.Json;
using System.Text;

namespace Assessment_Frontend.Services
{
    public class ApiService: IApiService
    {
        private readonly HttpClient _client;
        public ApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProductResponse> GetProducts(string search, int page, int pageSize)
        {
            var response = await _client.GetFromJsonAsync<ProductResponse>($"api/products?search={search}&page={page}&pageSize={pageSize}");
            return response;
        }

        public async Task AddToCart(int productId)
        {
            var cartItem = new CartItem { ProductId = productId, Quantity = 1 };
            var content = new StringContent(JsonSerializer.Serialize(cartItem), Encoding.UTF8, "application/json");
            await _client.PostAsync("api/cart/items", content);
        }

        public async Task<List<Cart>> GetCart()
        {
            var response = await _client.GetFromJsonAsync<List<Cart>>("api/cart");
            return response ?? new List<Cart>();
        }

        public async Task AddProductAsync(Product productDto)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(productDto.Name), "Name");
            form.Add(new StringContent(productDto.Description), "Description");
            form.Add(new StringContent(productDto.Price.ToString()), "Price");
            form.Add(new StringContent(productDto.DiscountPercentage.ToString()), "DiscountPercentage");

            if (productDto.DiscountStartDate.HasValue)
                form.Add(new StringContent(productDto.DiscountStartDate.Value.ToString("o")), "DiscountStartDate");

            if (productDto.DiscountEndDate.HasValue)
                form.Add(new StringContent(productDto.DiscountEndDate.Value.ToString("o")), "DiscountEndDate");

            if (productDto.Image != null)
            {
                var streamContent = new StreamContent(productDto.Image.OpenReadStream());
                form.Add(streamContent, "Image", productDto.Image.FileName);
            }

            var response = await _client.PostAsync("api/products", form);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }

            response.EnsureSuccessStatusCode();
        }

    }
}
