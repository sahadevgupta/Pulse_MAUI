using System;

namespace Pulse_MAUI.Helpers
{
	/// <summary>
	/// Class with all PCA specific fontsizes to use in the application. 
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class FontSizes
	{
		/// <summary>
		/// The micro font size.
		/// </summary>
		public static readonly double Micro = ScaleFontSize(12);

		/// <summary>
		/// The small font size.
		/// </summary>
		public static readonly double Small = ScaleFontSize(14);

		/// <summary>
		/// The medium font size.
		/// </summary>
		public static readonly double Medium = ScaleFontSize(16);

		/// <summary>
		/// The default font size.
		/// </summary>
		public static readonly double Default = ScaleFontSize(18);

		/// <summary>
		/// The large font size.
		/// </summary>
		public static readonly double Large = ScaleFontSize(21);

		private static double Density
				=> DeviceDisplay.MainDisplayInfo.Density;

		public static double ScaleFontSize(double size)
		{
			double multiplier = GetDensityMultiplier();
			double scaled = size * multiplier;

			return Math.Clamp(scaled, 8, 80);
		}
		private static double GetDensityMultiplier()
		{
			double width = DeviceDisplay.MainDisplayInfo.Width / Density;

			return width switch
			{
				<= 360 => 0.90,                 // small phone
				<= 400 => 0.90,                 // normal phone
				<= 480 => 0.90,                 // large phone
				<= 600 => 0.90,                 // phablet
				<= 840 => 0.90,                 // small tablet / PDA
				_ => 0.90,                 // large tablet / Desktop
			};
		}


	}
}
