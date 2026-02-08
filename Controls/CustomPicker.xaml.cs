#if ANDROID
using static Android.Views.ViewGroup;
#endif

using System.Collections;

namespace Pulse_MAUI.Controls;

public partial class CustomPicker : ContentView
{
    public static readonly BindableProperty TitleProperty = 
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomPicker));

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CustomPicker), default(IList),BindingMode.TwoWay);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomPicker),null, BindingMode.TwoWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public CustomPicker()
	{
		InitializeComponent();
		ModifyControl();
	}

    private void ModifyControl()
    {
		Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(CustomPicker), (handler, view) =>
		{
#if ANDROID
            var control = handler.PlatformView;
            control.Background = null;

            //var layoutParams = new MarginLayoutParams(control.LayoutParameters);
            //layoutParams.SetMargins(0, 0, 0, 0);
            //control.LayoutParameters = layoutParams;
            //control.LayoutParameters = layoutParams;
            //control.SetPadding(0, 0, 0, 0);
            control.SetPadding(0, 0, 0, 0);
#elif IOS
           handler.PlatformView.Layer.BorderWidth = 0;
           handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif

        });
    }
}