using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class PunchPage : BasePage
{
	public PunchPage(PunchPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override bool OnBackButtonPressed()
	{
		if (BindingContext is PunchPageViewModel vm)
		{
			this.Dispatcher.Dispatch(async () =>
			{
				await vm.OnBackPressed();
			});

		}
		return true;
	}
}