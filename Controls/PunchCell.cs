using System;
using Pulse_MAUI.Helpers;
using MauiColor = Microsoft.Maui.Graphics.Colors;
using Pulse_MAUI.Helpers;
using Colors = Pulse_MAUI.Helpers.Colors;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// ViewCell for PunchItems for the assigned punchitem list
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class PunchCell : ViewCell
	{
		CustomLabel punchId, text, detail;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Pulse_MAUI.Controls.PunchCell"/> class.
		/// </summary>
		public PunchCell()
		{
			SetupUserInterface();
			SetupBindings();
		}

		/// <summary>
		/// Sets up the user interface.
		/// </summary>
		private void SetupUserInterface()
		{

            punchId = new CustomLabel
            {
                TextColor = Colors.StockLightBlue,
                FontSize = FontSizes.Default,
                HorizontalOptions = LayoutOptions.Start,
            }; 

            text = new CustomLabel
			{
				TextColor = Colors.StockLightBlue,
				FontSize = FontSizes.Default,
				HorizontalOptions = LayoutOptions.End,
			};

			detail = new CustomLabel
			{
				TextColor = MauiColor.Gray,
				FontSize = FontSizes.Medium,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start,
			};

			Grid punchCellGrid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
					new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
					new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                },
				Padding = new Thickness(10, 10, 10, 0),
			};

            punchCellGrid.Add(punchId, 0, 0);
            punchCellGrid.Add(text,1, 0);
            punchCellGrid.Add(detail, 0,1);

            Grid.SetColumnSpan(detail, 2);

            View = punchCellGrid;
            
		}

		/// <summary>
		/// Sets up the bindings.
		/// </summary>
		private void SetupBindings()
		{
            punchId.SetBinding(Label.TextProperty, "PunchId");
            text.SetBinding(Label.TextProperty, "Title");
            detail.SetBinding(Label.TextProperty, "SubTitle");
		}
	}
}
