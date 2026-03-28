using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ImportSettingsPage : BasePage
{
	public ImportSettingsPage(ImportSettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}