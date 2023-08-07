using Microsoft.Toolkit.Mvvm.Input;

namespace Market.Model
{
    public class Product
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }

        public Command BuyButtonClickCommand { get; set; }

        public Product()
        {
            BuyButtonClickCommand = new Command(async () => await BuyButtonClickCommandExecute(Name));
        }

        private async Task BuyButtonClickCommandExecute(string sender)
        {
            await Shell.Current.DisplayAlert("Alert", !string.IsNullOrEmpty(sender) ? sender : "No item", "OK");
        }

    }
}