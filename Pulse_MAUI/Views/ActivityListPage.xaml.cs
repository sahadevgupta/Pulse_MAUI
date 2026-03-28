using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ActivityListPage : BasePage
{
	readonly ActivityListPageViewModel _viewModel;

	public ActivityListPage(ActivityListPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		//_viewModel.LoadDataOnAppearing();
	}


}