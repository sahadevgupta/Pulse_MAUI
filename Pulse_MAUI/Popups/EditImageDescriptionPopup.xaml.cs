using Mopups.Pages;
using Mopups.Services;
using static Pulse_MAUI.Controls.CustomDialogs;

namespace Pulse_MAUI.Popups;

public partial class EditImageDescriptionPopup : PopupPage
{
	internal event EventHandler<DetailInputResult>? OkClicked;
	public EditImageDescriptionPopup(int stepCount, string existingText, int? exitstingStep)
	{
		InitializeComponent();
		descriptionEntry.Text = existingText;

		var items = new List<string>();
		items.Add("None");

		// increment the step counter
		for (int i = 1; i < stepCount + 1; i++)
		{
			items.Add(i.ToString());
		}
		checklistPicker.ItemsSource = items;

		if (exitstingStep != null)
		{
			checklistPicker.SelectedItem = exitstingStep.ToString();
		}
		else
		{
			checklistPicker.SelectedItem = "None";
		}
		descriptionEntry.Focus();
	}

	private void Ok_Clicked(object sender, EventArgs e)
	{
		var result = new DetailInputResult();
		result.Description = descriptionEntry.Text;
		result.Step = checklistPicker.SelectedItem.ToString() ?? string.Empty;

		OkClicked?.Invoke(sender, result);
		MopupService.Instance.PopAsync();

	}

	private void Cancel_Clicked(object sender, EventArgs e)
	{
		MopupService.Instance.PopAsync();
	}
}