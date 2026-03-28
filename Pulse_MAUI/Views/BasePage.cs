using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Pulse_MAUI.ViewModels;
using Application = Microsoft.Maui.Controls.Application;

namespace Pulse_MAUI.Views;

public class BasePage : ContentPage
{
	public BasePage()
	{
		ApplyStatusBarStyle();
	}

	#region [ Methods ]
	private void ApplyStatusBarStyle()
	{
		this.Behaviors.Add(new StatusBarBehavior
		{
			StatusBarColor = Colors.Black,
			StatusBarStyle = StatusBarStyle.LightContent
		});

		if (OperatingSystem.IsIOS())
		{
			var safeInsects = On<iOS>().SafeAreaInsets();
			if (safeInsects.Top <= 0)
			{
				On<iOS>().SetUseSafeArea(true);
			}
			else
			{
				this.Padding = new Thickness(0, safeInsects.Top, 0, 0);
			}
		}
	}
	#endregion

	#region [ Override Methods ]

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is BaseViewModel viewModel)
		{
			await viewModel.LoadDataOnAppearing();
		}
	}

	protected override async void OnDisappearing()
	{
		base.OnDisappearing();
		if (BindingContext is BaseViewModel viewModel)
		{
			await viewModel.LoadDataOnDisappearing();
		}
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		if (BindingContext is BaseViewModel viewModel && args.NavigationType != NavigationType.Pop)
		{
			await viewModel.LoadDataOnNavigatedTo();
		}
	}
	#endregion
}