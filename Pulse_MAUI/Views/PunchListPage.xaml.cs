using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class PunchListPage : BasePage
{
	public PunchListPage(PunchListPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}