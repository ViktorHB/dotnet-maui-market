namespace Market.ViewModel
{
    [QueryProperty("Product", "Product")]
    public partial class ProductDetailsPageViewModel : ViewModelBase
    {
        [ObservableProperty] 
        private Product product;
    }
}
