using Microsoft.Toolkit.Mvvm.Input;

namespace Market.ViewModel
{
    [QueryProperty("Product", "Product")]
    public partial class ProductDetailsPageViewModel : ViewModelBase
    {
        [ObservableProperty] 
        private Product product;

        [ICommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
