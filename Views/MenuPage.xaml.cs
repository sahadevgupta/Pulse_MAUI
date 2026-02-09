using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class MenuPage : ContentView
{
	public MenuPage()
	{
		InitializeComponent();
		var viewModel = IPlatformApplication.Current?.Services.GetService<MenuPageViewModel>();
		BindingContext = viewModel;
	}
}