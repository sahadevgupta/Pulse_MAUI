using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Views;

public partial class FileListPage : BasePage
{
	public FileListPage(FileListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}