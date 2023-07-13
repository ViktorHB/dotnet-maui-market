using Market.Model;
using Microsoft.Maui.Controls;

namespace Market;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Button_OnClicked(object sender, EventArgs e)
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
                            await DisplayAlert("Alert", lab.Text, "OK");
                            return;
                        }
                    }
                }
            }
        }

        await DisplayAlert("Alert", "No item", "OK");
    }
}

