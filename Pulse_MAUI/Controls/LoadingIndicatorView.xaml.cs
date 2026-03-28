namespace Pulse_MAUI.Controls;

public partial class LoadingIndicatorView : ContentView
{
	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(LoadingIndicatorView),defaultBindingMode: BindingMode.TwoWay);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}
	public LoadingIndicatorView()
	{
		InitializeComponent();
	}
}