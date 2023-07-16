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

        [ObservableProperty] public ObservableCollection<Product> products;
        [ObservableProperty] public Product selectedProduct;

        public MainPageViewModel(ProductService productService)
        {
            Title = "Products";
            _productService = productService;
            GetProductsCommand.Execute(this);
        }

        private async Task GetProductsCommandExecute()
        {
            await GetProductsAsync();
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