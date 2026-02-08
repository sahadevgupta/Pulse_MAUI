using System;

using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.ViewModels;

namespace Pulse_MAUI.Controls
{
	public class ProfileView : ContentView
	{
		CustomLabel name, date;
		Color fontColor;

		public ProfileView(IDialogService dialogService,Color fontColor)
		{
			this.fontColor = fontColor;

			BindingContext = new ProfilePageViewModel(dialogService);

			SetupUserInterface();
			SetupBindings();
		}

		private void SetupUserInterface()
		{
			name = new CustomLabel
			{
				TextColor = fontColor,
				FontSize = FontSizes.Small,
				HorizontalTextAlignment = TextAlignment.End
			};

			date = new CustomLabel
			{
				TextColor = fontColor,
				FontSize = FontSizes.Small,
				HorizontalTextAlignment = TextAlignment.End
			};

			var profileLayout = new StackLayout
			{
				Children = { name, date },
				VerticalOptions = LayoutOptions.End,
				HorizontalOptions = LayoutOptions.End,
			};

			Grid headerGrid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
				},
				Padding = new Thickness(10, 10, 10, 10),
			};

			headerGrid.Add(profileLayout, 0, 0);

			Content = headerGrid;
		}

		private void SetupBindings()
		{
			name.SetBinding(Label.TextProperty, new Binding(nameof(ProfilePageViewModel.ProfileName)));
			date.SetBinding(Label.TextProperty, new Binding(nameof(ProfilePageViewModel.CurrentDate)));
		}
	}
}
