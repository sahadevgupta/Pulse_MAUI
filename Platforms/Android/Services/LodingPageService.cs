using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Microsoft.Maui.Platform;
using Pulse_MAUI.Controls;
using Pulse_MAUI.Interfaces;
using Application = Microsoft.Maui.Controls.Application;
using View = Android.Views.View;
using Window = Android.Views.Window;

namespace Pulse_MAUI.Platforms.Android.Services
{
    public class LodingPageService : ILoadingService
    {
        View? _nativeView;
        private Dialog? _dialog;
        private bool _isInitialized;
        public LodingPageService()
        {
            InitLoadingPage();
        }

        private void InitLoadingPage()
        {
            if(Application.Current?.Windows.Any() == false)
                return;

            var page = Application.Current?.Windows[0].Page;
            var loadingView = new LoadingIndicatorView();
            if (page?.Handler != null)
            {
                _nativeView = loadingView.ToHandler(page.Handler?.MauiContext!)?.PlatformView;

                _dialog = new Dialog(Platform.CurrentActivity);
                _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                _dialog.SetCancelable(false);
                _dialog.SetContentView(_nativeView);
                Window window = _dialog.Window;
                window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.SetBackgroundDrawable(new ColorDrawable(Colors.Transparent.ToPlatform()));

                _isInitialized = true;
            }
        }

        public void HideLoading()
        {
            // Hide the page
            _dialog?.Hide();
        }

        public void ShowLoading()
        {
            if (!_isInitialized)
                InitLoadingPage(); // set the default page

            // showing the native loading page
            _dialog?.Show();
        }
    }
}
