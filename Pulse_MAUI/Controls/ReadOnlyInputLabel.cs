using Pulse_MAUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Controls
{
    /// <summary>
    /// Create a custom input lable in ReadOnly style
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Label" />
    public class ReadOnlyInputLabel : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyInputLabel"/> class.
        /// </summary>
        public ReadOnlyInputLabel()
        {
            this.FontFamily = "SignikaRegular";
            // this.FontSize = FontSizes.Medium;
            // this.BackgroundColor = Color.White;
            // this.VerticalOptions = LayoutOptions.Fill;
            // this.HeightRequest = 240;
            // this.Margin = new Thickness(0, 0, 0, 0);

        }

    }
}
