using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Microsoft.Identity.Client;

namespace Pulse_MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetupDeviceDensityFormula();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);

        }

        /// <summary>
        /// Pulls out the device specific setttings and works out the font size formula.
        /// </summary>
        private void SetupDeviceDensityFormula()
        {
            DisplayMetrics displayMetrics = Android.App.Application.Context.Resources.DisplayMetrics;
            var densityDpi = (int)displayMetrics.DensityDpi;
            var xDpi = (float)displayMetrics.Xdpi;
            var topMultiplier = Convert.ToDouble(densityDpi) / 480;
            App.SetSizeFormula(topMultiplier, xDpi);
        }

    }
}
