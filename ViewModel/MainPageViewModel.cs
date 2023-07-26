using Market.Services;
using Market.View;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Market.ViewModel
{
    public partial class MainPageViewModel : ViewModelBase
    {
        private readonly ProductService _productService;
        private readonly IConnectivity _connectivity;
        private readonly IGeolocation _geolocation;

        [ObservableProperty] public ObservableCollection<Product> products;
        [ObservableProperty] public Product selectedProduct;
        [ObservableProperty] public bool isRefreshing;

        public MainPageViewModel(ProductService productService, IConnectivity connectivity, IGeolocation geolocation)
        {
            TextToSpeech.SpeakAsync("Welcome", CancellationToken.None);
            Title = "Products";
            _productService = productService;
            _connectivity = connectivity;
            _geolocation = geolocation;
            GetProductsCommand.Execute(this);
        }

        private async Task GetProductsCommandExecute()
        {
            await GetProductsAsync();
        }

        [ICommand]
        private async Task RefreshAsync()
        {
            try
            {
                IsBusy = true;
                IsRefreshing = true;
                GetProductsCommand.Execute(this);
                IsBusy = false;
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("error", ex.Message, "Ok");
            }
        }

        [ICommand]
        private async Task GetProductsAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet issue", "No internet connection", "Ok");
                    return;
                }

                IsBusy = true;
                Products = new ObservableCollection<Product>(await _productService.GetProductsAsync());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task GetClosestMarketAsync()
        {
            if (IsBusy || Products.Count == 0)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var isLocationPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (isLocationPermission != PermissionStatus.Granted)
                {
                    return;
                }

                var location = await _geolocation.GetLastKnownLocationAsync();

                if (location is null)
                {
                    location = await _geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(5)
                    });
                }

                if (location is null)
                {
                    return;
                }

                var closest = Products
                    .OrderBy(x => location.CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Kilometers))
                    .FirstOrDefault();

                if (closest is null)
                {
                    return;
                }

                await Shell.Current.DisplayAlert("Closest market", $"{closest.Name} in {closest.Location}", "OK");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert("Error", $"Unable to get closest market {e.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task GoToDetailsAsync(Product product)
        {
            if (product is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(ProductDetailsPage), true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }


        [ICommand]
        private async Task ProductClickAsync(object sender)
        {
            if (sender is Button button)
            {
                if (button.Parent is HorizontalStackLayout par)
                {
                    if (par.Children.First(x => x.GetType() == typeof(VerticalStackLayout)) is VerticalStackLayout vert)
                    {
                        foreach (var item in vert.Children)
                        {
                            if (item is Label lab)
                            {
                                await Shell.Current.DisplayAlert("Alert", lab.Text, "OK");
                                return;
                            }
                        }
                    }
                }
            }

            GetProductsCommand.Execute(this);
            await Shell.Current.DisplayAlert("Order", "Order", "OK", "CANCEL");
        }
    }
}