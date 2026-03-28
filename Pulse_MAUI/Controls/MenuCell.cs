using Pulse_MAUI.Helpers;
using MauiColor = Microsoft.Maui.Graphics.Colors;

using Microsoft.Maui;
using Colors = Pulse_MAUI.Helpers.Colors;
using Pulse_MAUI.Models;
using Microsoft.Maui.Controls;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Menu Listview Cell.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class MenuCell : ViewCell
	{
		CustomLabel titleLabel;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Pulse_MAUI.Controls.MenuCell"/> class.
		/// </summary>
		public MenuCell()
		{
			SetupUserInterface();
			SetupBindings();
		}

		/// <summary>
		/// Sets up the user interface.
		/// </summary>
		private void SetupUserInterface()
		{
            titleLabel = new CustomLabel
            {
                BackgroundColor = MauiColor.Transparent,
                TextColor = Colors.StockLightBlue,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = FontSizes.Large
            };

			var mainLayout = new StackLayout
			{
				Children = { titleLabel },
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(10, 0, 10, 0),
                Spacing = 20
			};

			View = mainLayout;
		}

		/// <summary>
		/// Sets up the bindings.
		/// </summary>
		private void SetupBindings()
		{
            titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(MenuOption.Title)));
        }
	}
}
