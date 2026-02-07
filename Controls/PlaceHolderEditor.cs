using System;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Xamarin Forms Editor with a placeholder.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class PlaceholderEditor : Editor
	{
		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create(nameof(Placeholder),typeof(string),typeof(PlaceholderEditor),string.Empty);

		/// <summary>
		/// Gets or sets the placeholder.
		/// </summary>
		/// <value>The placeholder.</value>
		public string Placeholder
		{
			get
			{
				return (string)GetValue(PlaceholderProperty);
			}

			set
			{
				SetValue(PlaceholderProperty, value);
			}
		}
	}
}
