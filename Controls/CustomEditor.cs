using System;

using Pulse_MAUI.Helpers;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Editor with PCA specific styling.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomEditor : PlaceholderEditor
	{
        static int? _maxLength = null;
        public static new int? MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }


        public CustomEditor()
		{
            FontFamily = "SignikaRegular";
            FontSize = FontSizes.Default;

            this.TextChanged += (sender, args) =>
            {
                CustomEditor _entry = (CustomEditor)sender;

                if (_maxLength == null)
                {
                    return;
                }
                else
                {
                    string _text = _entry.Text;      //Get Current Text
                    if (_text.Length > _maxLength)       //If it is more than your character restriction
                    {
                        _text = _text.Remove(_text.Length - 1);  // Remove Last character
                        _entry.Text = _text;        //Set the Old value
                    }
                }

            };
        }

        /// <summary>
        /// The maximum length property
        /// </summary>
        public static new BindableProperty MaxLengthProperty =
         BindableProperty.Create(
         propertyName: nameof(MaxLength),
         returnType: typeof(int?),
         declaringType: typeof(CustomEditor),
         defaultValue: 123,
         defaultBindingMode: BindingMode.TwoWay,
         propertyChanged: OnEventMaxLengthChanged);

        /// <summary>
        /// Called when [event maximum length changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnEventMaxLengthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditor)bindable;
            MaxLength = (int?)newValue;

        }
    }
}
