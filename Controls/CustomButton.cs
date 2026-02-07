using Pulse_MAUI.Helpers;
using Colors = Pulse_MAUI.Helpers.Colors;
using MauiColors = Microsoft.Maui.Graphics.Colors;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Button with PCA specific styling.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomButton : Button
	{
		public CustomButton()
		{
			BorderColor = Colors.StandardOrange;
			TextColor = MauiColors.White;
			BackgroundColor = Colors.StandardOrange;
			BorderWidth = 1;
			FontFamily = "SignikaRegular";
		}
	}
}
