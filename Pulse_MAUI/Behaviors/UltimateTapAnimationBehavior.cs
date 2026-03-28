using System;
using System.Windows.Input;

namespace Pulse_MAUI.Behaviors;

public class UltimateTapAnimationBehavior : Behavior<View>
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(UltimateTapAnimationBehavior));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(UltimateTapAnimationBehavior));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    public double PressedScale { get; set; } = 0.95;
    public double PressedOpacity { get; set; } = 0.4;
    public uint AnimationDuration { get; set; } = 300;

    private TapGestureRecognizer _tap;

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);
        BindingContext = bindable.BindingContext; // Add this line
        bindable.BindingContextChanged += OnBindingContextChanged;
        // Add tap recognizer
        _tap = new TapGestureRecognizer();
        _tap.Tapped += async (s, e) => await Animate(bindable);
        bindable.GestureRecognizers.Add(_tap);
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
            BindingContext = bindable.BindingContext;
    }

    private async Task Animate(View view)
    {
        try
        {
            // Fade + Scale (pressed)
            await Task.WhenAll(
                view.FadeToAsync(PressedOpacity, AnimationDuration, Easing.CubicOut),
                view.ScaleToAsync(PressedScale, AnimationDuration, Easing.CubicOut)
            );

            await Task.Delay(50); // Small delay to enhance the effect

            // Fade + Scale (release)
            await Task.WhenAll(
                view.FadeToAsync(1, AnimationDuration, Easing.CubicIn),
                view.ScaleToAsync(1, AnimationDuration, Easing.CubicIn)
            );

            if (Command?.CanExecute(CommandParameter) == true)
                Command.Execute(CommandParameter);
        }
        catch { }
    }

    protected override void OnDetachingFrom(View bindable)
    {
        bindable.BindingContextChanged -= OnBindingContextChanged;
        bindable.GestureRecognizers.Remove(_tap);
        base.OnDetachingFrom(bindable);
    }
}

