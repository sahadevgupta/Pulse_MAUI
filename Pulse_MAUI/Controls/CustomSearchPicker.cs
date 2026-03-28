using System;

namespace Pulse_MAUI.Controls
{
    /// <summary>
    /// Search Picker with PCA specific styling (through custom renderer).
    /// Fogbugz Case:
    /// Author: Manuel Dambrine
    /// Created: 29/03/2013
    /// </summary>
    public class CustomSearchPicker : Picker
    {
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                this.InvalidateMeasure();
            }
        }
    }
}
