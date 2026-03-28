using System;
using FFImageLoading.Maui;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Image with a tapgesturerecognizer attached.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomImage : CachedImage
	{
		TapGestureRecognizer imageTapRecognizer;

		public TapGestureRecognizer ImageTapRecognizer
		{
			get
			{
				return imageTapRecognizer;
			}
			set
			{
				imageTapRecognizer = value;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImage"/> class.
        /// </summary>
        public CustomImage()
		{
			imageTapRecognizer = new TapGestureRecognizer();
			this.GestureRecognizers.Add(imageTapRecognizer);
		}
	}
}
