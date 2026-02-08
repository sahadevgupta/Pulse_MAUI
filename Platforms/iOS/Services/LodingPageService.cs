using Microsoft.Maui.Platform;
using Pulse_MAUI.Controls;
using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using WebKit;

namespace Pulse_MAUI.Platforms.iOS.Services
{
    public class LodingPageService : ILoadingService
    {
        private UIView _nativeView;
        private bool _isInitialized;
        public LodingPageService()
        {
            InitLoadingPage();
        }
        public void HideLoading()
        {
            // Hide the page
            _nativeView.RemoveFromSuperview();
        }

        public void ShowLoading()
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(); // set the default page

            // showing the native loading page
            UIApplication.SharedApplication.KeyWindow?.AddSubview(_nativeView);
        }

        private void InitLoadingPage()
        {
            var page = Application.Current?.Windows[0].Page;
            var loadingView = new LoadingIndicatorView();
            if (page?.Handler != null)
            {
                loadingView.Arrange(new Rect(0,0, page.Width, page.Height));
                _nativeView = loadingView.ToHandler(page.Handler?.MauiContext!)?.PlatformView;

                _isInitialized = true;
            }
        }
    }
}
