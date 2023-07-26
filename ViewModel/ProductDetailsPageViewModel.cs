using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;

namespace Market.ViewModel
{
    [QueryProperty("Product", "Product")]
    public partial class ProductDetailsPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Product product;

        private readonly IMap _map;

        public ProductDetailsPageViewModel(IMap map)
        {
            _map = map;
        }

        [ICommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [ICommand]
        private async Task OpenMapAsync()
        {
            try
            {
                await _map.OpenAsync(Product.Latitude, Product.Longitude,
                    new MapLaunchOptions
                    {
                        Name = Product.Name,
                        NavigationMode = NavigationMode.None
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("error", $"Unable to open map{ex.Message}", "Ok");
            }

        }
    }
}
