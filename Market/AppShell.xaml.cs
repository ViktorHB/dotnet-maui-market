using Market.View;

namespace Market;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		//Register route page in shell
		Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
	}
}
