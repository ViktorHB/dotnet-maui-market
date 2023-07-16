namespace Market.View;

public partial class ProductDetailsPage : ContentPage
{
    public ProductDetailsPage(ProductDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}