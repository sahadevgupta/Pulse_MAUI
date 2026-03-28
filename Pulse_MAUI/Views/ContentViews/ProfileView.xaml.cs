namespace Pulse_MAUI.Views.ContentViews;

public partial class ProfileView : ContentView
{
	public ProfileView()
	{
		InitializeComponent();
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        var a = this.BindingContext;
    }
}