using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class MenuPage : ContentView
{
	readonly MenuPageViewModel? viewModel;
	public MenuPage()
	{
		InitializeComponent();
		viewModel = IPlatformApplication.Current?.Services.GetService<MenuPageViewModel>();
		BindingContext = viewModel;
	}

	protected override async void OnParentSet()
	{
		base.OnParentSet();
		if (Parent != null && viewModel != null)
		{
			await viewModel.LoadDataOnNavigatedTo();
		}
	}

}