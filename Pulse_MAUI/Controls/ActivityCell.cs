using Microsoft.Maui;
using Pulse_MAUI.Helpers;
using Colors = Pulse_MAUI.Helpers.Colors;
using MauiColor = Microsoft.Maui.Graphics.Colors;
namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Class defining the ViewCell for an activity Listview.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class ActivityCell : ViewCell
	{
		CustomLabel activityId,text, detail,status;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Pulse_MAUI.Controls.ActivityCell"/> class.
		/// </summary>
		public ActivityCell()
		{
			SetupUserInterface();
			SetupBindings();
		}

		/// <summary>
		/// Setups the user interface.
		/// </summary>
		private void SetupUserInterface()
		{
            activityId = new CustomLabel
            {   
                Text = "ID:",
                TextColor = Colors.StockLightBlue,
                FontSize = FontSizes.Default,
                HorizontalOptions = LayoutOptions.Start,
            };

            text = new CustomLabel
			{
				TextColor = Colors.StockLightBlue,
				FontSize = FontSizes.Default,
				HorizontalOptions = LayoutOptions.Start,
			};

			detail = new CustomLabel
			{
				TextColor = MauiColor.Gray,
				FontSize = FontSizes.Medium,
				HorizontalOptions = LayoutOptions.Start,
			};

            status = new CustomLabel
            {
                TextColor = MauiColor.Gray,
                FontSize = FontSizes.Medium,
                HorizontalOptions = LayoutOptions.End,
            };

            Grid punchCellGrid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
					new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength (0.5, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (0.5, GridUnitType.Star) }
				},
				Padding = new Thickness(10, 10, 10, 0),
			};

    

            punchCellGrid.Add(activityId, 0, 0);
            punchCellGrid.Add(text, 0, 1);

			punchCellGrid.Add(detail, 0, 2);
            punchCellGrid.Add(status, 1, 2);

            Grid.SetColumnSpan(text, 2);

            View = punchCellGrid;


		}

		/// <summary>
		/// Setups the bindings.
		/// </summary>
		private void SetupBindings()
		{
            
            activityId.SetBinding(Label.TextProperty, "PCAId");
            text.SetBinding(Label.TextProperty, "Name");
            detail.SetBinding(Label.TextProperty, "TagId");
            status.SetBinding(Label.TextProperty, "Status");
        }
	}
}
