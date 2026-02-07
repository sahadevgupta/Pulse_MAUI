using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class ImportSettingsPage : ContentPage
{
	public ImportSettingsPage(ImportSettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}