using System;

using Pulse_MAUI.Helpers;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Entry with PCA specific styling.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomEntry : Entry
	{
		public CustomEntry()
		{
            FontFamily = "SignikaRegular";
            FontSize = FontSizes.Default;
		}
	}
}
