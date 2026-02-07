using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Controls
{
    /// <summary>
    /// Creates a Custom Dialog
    /// </summary>
    class CustomDialogs
    {
        public struct DetailInputResult
        {
            public string Description { get; set; }
            public string Step { get; set; }
        }

        /// <summary>
        /// Items the detail input box.
        /// </summary>
        /// <param name="navigation">The navigation.</param>
        /// <param name="stepCount">The step count.</param>
        /// <param name="existingText">The existing text.</param>
        /// <param name="exitstingStep">The exitsting step.</param>
        /// <returns></returns>
        public static Task<DetailInputResult> ItemDetailInputBox(INavigation navigation, int stepCount, string existingText, int? exitstingStep)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<DetailInputResult> ();

            var lblTitle = new Label { Text = "File Item Details", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 24, Margin = new Thickness(20) };


            var lblMessage = new Label { Text = "Please Enter Description", Margin = new Thickness(0,10,0,0) };
            var txtInput = new Editor { Text = existingText };


            var lblStepMessage = new Label { Text = "Please Select Checklist Step", Margin = new Thickness(0, 40, 0,0) };
            var pickStep = new Picker { Title = "Checklist Step", VerticalOptions = LayoutOptions.CenterAndExpand };


            pickStep.Items.Add("None");
            // increment the step counter
            for (int i = 1; i < stepCount + 1; i++)
            {
                pickStep.Items.Add(i.ToString());
            }

            if (exitstingStep != null)
            {
                pickStep.SelectedItem = exitstingStep.ToString();
            } else
            {
                pickStep.SelectedItem = "None";
            }

            var btnOk = new CustomButton
            {
                Text = "Ok",
                WidthRequest = 100
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = new DetailInputResult();
                result.Description = txtInput.Text;
                result.Step = pickStep.SelectedItem.ToString();
               
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new CustomButton
            {
                Text = "Cancel",
                WidthRequest = 100
            };
            btnCancel.Clicked += async (s, e) =>
            {
                var result = new DetailInputResult();
                result.Description = null;
                result.Step = null;

               // close page
               await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(result);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput,lblStepMessage, pickStep, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }

    }
}
