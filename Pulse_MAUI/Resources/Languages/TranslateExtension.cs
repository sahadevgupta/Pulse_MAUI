using System;
using System.ComponentModel;
using System.Globalization;

namespace Pulse_MAUI.Resources.Languages
{
	/// <summary>
	/// Extension class used for handling transation of text. Used in XAML definitions.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	[ContentProperty("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }

		/// <summary>
		/// Provides the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="serviceProvider">Service provider.</param>
		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Text == null)
				return null;

			return UserInterface.ResourceManager.GetString(Text, CultureInfo.CurrentCulture);
		}
	}
}
