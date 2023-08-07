
using System.Text.Json;

namespace Market.Services
{
    public class ProductService
    {
        private List<Product> _products = new ();

        public async Task<List<Product>> GetProductsAsync()
        {
            if (_products?.Count > 0)
            {
                return _products;
            }

            await using var stream = await FileSystem.OpenAppPackageFileAsync("Products.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();

            _products = JsonSerializer.Deserialize<List<Product>>(contents);

            return _products;
        }
    }
}
