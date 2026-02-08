using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ActivityListPage : ContentPage
{
	public ActivityListPage(ActivityListPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}