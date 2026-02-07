using System;

using Pulse_MAUI.Helpers;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Label with PCA specific styling.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomLabel : Label
	{
		public CustomLabel()
		{
            FontFamily = "SignikaRegular";
            FontSize = FontSizes.Default;
		}
	}
}
