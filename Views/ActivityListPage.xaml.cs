using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ActivityListPage : ContentPage
{
	readonly ActivityListPageViewModel _viewModel;

	public ActivityListPage(ActivityListPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.RefreshActivityListCommand.ExecuteAsync(null);
	}
}