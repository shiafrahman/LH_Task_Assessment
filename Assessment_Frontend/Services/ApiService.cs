using Assessment_Frontend.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

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
            return response ?? new ProductResponse();
        }

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                return await _client.GetFromJsonAsync<Product>($"api/products/{id}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task AddToCart(int productId)
        {
            var cartItem = new { ProductId = productId, Quantity = 1 };
            var content = new StringContent(JsonSerializer.Serialize(cartItem), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/cart", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCart(int productId)
        {
            var response = await _client.DeleteAsync($"api/cart/{productId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<CartResponse>> GetCart()
        {
            var response = await _client.GetFromJsonAsync<CartResponseWrapper>("api/cart");
            return response?.Items ?? new List<CartResponse>();
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
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "Image",
                    FileName = productDto.Image.FileName
                };
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(productDto.Image.ContentType);
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

        public async Task UpdateProductAsync(Product productDto)
        {
            var form = new MultipartFormDataContent();

            form.Add(new StringContent(productDto.Id.ToString()), "Id");
            form.Add(new StringContent(productDto.Name ?? ""), "Name");
            form.Add(new StringContent(productDto.Description ?? ""), "Description");
            form.Add(new StringContent(productDto.Price.ToString()), "Price");
            form.Add(new StringContent(productDto.DiscountPercentage?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "0"), "DiscountPercentage");

            if (productDto.DiscountStartDate.HasValue)
                form.Add(new StringContent(productDto.DiscountStartDate.Value.ToString("o")), "DiscountStartDate");

            if (productDto.DiscountEndDate.HasValue)
                form.Add(new StringContent(productDto.DiscountEndDate.Value.ToString("o")), "DiscountEndDate");

            if (productDto.Image != null)
            {
                var streamContent = new StreamContent(productDto.Image.OpenReadStream());
                streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "Image",
                    FileName = productDto.Image.FileName
                };
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(productDto.Image.ContentType);
                form.Add(streamContent, "Image", productDto.Image.FileName);
            }

            var response = await _client.PutAsync($"api/products/{productDto.Id}", form);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/products/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to delete product: {ex.Message}");
            }
        }

    }
}
