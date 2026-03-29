using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ActivityPage : BasePage
{
	readonly ActivityPageViewModel _viewModel;

	public ActivityPage(ActivityPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}