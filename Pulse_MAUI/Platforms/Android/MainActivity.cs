using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Identity.Client;
using Microsoft.Maui.Platform;

namespace Pulse_MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity? Current { get; private set; }

        private AndroidX.AppCompat.Widget.Toolbar? _toolbar;
        private global::Android.Views.View? _statusBarScrim;
        private global::Android.Views.View? _flyoutStatusBarOverlay;
        private int _baseToolbarPaddingTop = -1;
        private int _baseToolbarMinHeight = -1;
        private bool _systemBarRefreshPending;
        private Page? _pendingRefreshPage;
        private global::Android.Graphics.Color? _lastAppliedChromeColor;
        private global::Android.Graphics.Color? _lastAppliedStatusBarScrimColor;
        private int _lastAppliedStatusBarInset = -1;
        private int _lastAppliedNavigationBarInset = -1;
        private int _lastToolbarAppliedPaddingTop = -1;
        private int _lastToolbarAppliedMinHeight = -1;
        private AndroidX.DrawerLayout.Widget.DrawerLayout? _drawerLayout;
        private global::Android.Views.View? _drawerFlyoutView;
        private int _lastAppliedFlyoutStatusBarInset = -1;
        private bool _windowInsetsListenerInstalled;
        private bool _drawerListenerInstalled;


        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EnsureWindowInsetsListener();
            ApplySystemBars();
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnsureWindowInsetsListener();
            ApplySystemBars();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);

        }
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
                ApplySystemBars();
        }

        private void EnsureWindowInsetsListener()
        {
            if (_windowInsetsListenerInstalled || Window?.DecorView is null)
                return;

            ViewCompat.SetOnApplyWindowInsetsListener(Window.DecorView, new WindowInsetsListener(this));
            ViewCompat.RequestApplyInsets(Window.DecorView);
            _windowInsetsListenerInstalled = true;
        }



        private void OnWindowInsetsChanged()
        {
            var currentPage = Shell.Current?.CurrentPage;
            var statusBarInset = GetStatusBarInset();

            if (_lastAppliedStatusBarInset != statusBarInset || _lastAppliedChromeColor is null)
                ApplySystemBars(currentPage);
            else
                ApplyFlyoutEdgeToEdge();

            ApplyPageInsets(currentPage);
        }



        private void ApplySystemBars(Page? page = null)
        {
            if (Window?.DecorView is null)
                return;

            WindowCompat.SetDecorFitsSystemWindows(Window, false);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.ClearFlags(WindowManagerFlags.TranslucentNavigation);

#pragma warning disable CA1422
            Window.SetStatusBarColor(global::Android.Graphics.Color.Transparent);
            Window.SetNavigationBarColor(global::Android.Graphics.Color.Transparent);
#pragma warning restore CA1422

            var resolvedPage = page ?? Shell.Current?.CurrentPage;
            var chromeColor = ResolveChromeColor(resolvedPage);
            var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
            if (controller is not null)
            {
                var useDarkIcons = IsLightColor(chromeColor);
                controller.AppearanceLightStatusBars = useDarkIcons;
                controller.AppearanceLightNavigationBars = useDarkIcons;
            }

            var statusBarScrimColor = ResolveStatusBarScrimColor(chromeColor);
            ApplyStatusBarScrim(statusBarScrimColor);
            ApplyToolbarChrome(chromeColor);
            ApplyFlyoutEdgeToEdge();
            _lastAppliedChromeColor = chromeColor;
        }

        private void ApplyFlyoutEdgeToEdge()
        {
            if (Window?.DecorView is null)
                return;

            var drawerLayout = _drawerLayout ??= FindDrawerLayout(Window.DecorView);
            if (drawerLayout is null)
                return;

            EnsureDrawerListener(drawerLayout);
            drawerLayout.SetClipToPadding(false);
            drawerLayout.SetStatusBarBackgroundColor(global::Android.Graphics.Color.ParseColor("#04273F"));

            for (var i = 0; i < drawerLayout.ChildCount; i++)
            {
                var child = drawerLayout.GetChildAt(i);
                if (child is null)
                    continue;

                child.SetPadding(child.PaddingLeft, 0, child.PaddingRight, child.PaddingBottom);

                if (child is ViewGroup group)
                    group.SetClipToPadding(false);
            }

            var flyoutView = _drawerFlyoutView ??= FindDrawerContentView(drawerLayout);
            var statusBarInset = GetStatusBarInset();

            if (flyoutView is not null)
            {
                if (flyoutView.LayoutParameters is ViewGroup.MarginLayoutParams marginLayoutParams && marginLayoutParams.TopMargin != 0)
                {
                    marginLayoutParams.TopMargin = 0;
                    flyoutView.LayoutParameters = marginLayoutParams;
                }
            }

            UpdateFlyoutStatusBarOverlay(drawerLayout, flyoutView, statusBarInset);

            if (_lastAppliedFlyoutStatusBarInset == statusBarInset)
                return;

            _lastAppliedFlyoutStatusBarInset = statusBarInset;
            var density = Resources?.DisplayMetrics?.Density ?? 1f;
            var topInsetDip = statusBarInset / density;

            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Shell.Current is AppShell appShell)
                    appShell.ApplyFlyoutTopInset(topInsetDip);
            });
        }

        private global::Android.Graphics.Color ResolveStatusBarScrimColor(global::Android.Graphics.Color pageChromeColor)
        {
            var drawerLayout = _drawerLayout ??= FindDrawerLayout(Window?.DecorView);
            var flyoutView = _drawerFlyoutView ?? (drawerLayout is null ? null : FindDrawerContentView(drawerLayout));

            if (drawerLayout is not null && flyoutView is not null && drawerLayout.IsDrawerOpen(flyoutView))
                return global::Android.Graphics.Color.ParseColor("#04273F");

            return pageChromeColor;
        }

        private void ApplyStatusBarScrim(global::Android.Graphics.Color chromeColor)
        {
            if (Window?.DecorView is not ViewGroup decorRoot)
                return;

            var statusBarInset = GetStatusBarInset();
            _lastAppliedStatusBarInset = statusBarInset;
            if (statusBarInset <= 0)
                return;

            if (_statusBarScrim is null || _statusBarScrim.Parent is null)
            {
                _statusBarScrim = new global::Android.Views.View(this)
                {
                    Clickable = false,
                    Focusable = false
                };

                var layoutParams = new global::Android.Widget.FrameLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    statusBarInset)
                {
                    Gravity = GravityFlags.Top
                };

                decorRoot.AddView(_statusBarScrim, decorRoot.ChildCount, layoutParams);
            }

            if (_statusBarScrim.LayoutParameters is global::Android.Widget.FrameLayout.LayoutParams frameLayoutParams)
            {
                if (frameLayoutParams.Height != statusBarInset)
                {
                    frameLayoutParams.Height = statusBarInset;
                    _statusBarScrim.LayoutParameters = frameLayoutParams;
                }
            }

            if (_lastAppliedStatusBarScrimColor is null || !_lastAppliedStatusBarScrimColor.Equals(chromeColor) || _statusBarScrim.Background is null)
            {
                _statusBarScrim.SetBackgroundColor(chromeColor);
                _lastAppliedStatusBarScrimColor = chromeColor;
            }

            _statusBarScrim.BringToFront();
        }

        private void ApplyToolbarChrome(global::Android.Graphics.Color chromeColor)
        {
            if (Window?.DecorView is null)
                return;

            var toolbar = FindToolbar(Window.DecorView);
            if (toolbar is null)
                return;

            if (!ReferenceEquals(_toolbar, toolbar))
            {
                _toolbar = toolbar;
                _baseToolbarPaddingTop = toolbar.PaddingTop;
                _baseToolbarMinHeight = toolbar.MinimumHeight > 0 ? toolbar.MinimumHeight : GetDefaultToolbarHeight();
            }

            var statusBarInset = GetStatusBarInset();
            var basePaddingTop = _baseToolbarPaddingTop >= 0 ? _baseToolbarPaddingTop : 0;
            var baseMinHeight = _baseToolbarMinHeight > 0 ? _baseToolbarMinHeight : GetDefaultToolbarHeight();

            var appliedPaddingTop = basePaddingTop + statusBarInset;
            var appliedMinHeight = baseMinHeight + statusBarInset;
            var requiresLayout = false;

            if (_lastAppliedChromeColor is null || !_lastAppliedChromeColor.Equals(chromeColor))
                ApplyColorToToolbarHierarchy(toolbar, chromeColor);

            if (_lastToolbarAppliedPaddingTop != appliedPaddingTop)
            {
                toolbar.SetPadding(toolbar.PaddingLeft, appliedPaddingTop, toolbar.PaddingRight, toolbar.PaddingBottom);
                _lastToolbarAppliedPaddingTop = appliedPaddingTop;
                requiresLayout = true;
            }

            if (_lastToolbarAppliedMinHeight != appliedMinHeight)
            {
                toolbar.SetMinimumHeight(appliedMinHeight);
                _lastToolbarAppliedMinHeight = appliedMinHeight;
                requiresLayout = true;
            }

            if (requiresLayout)
                toolbar.RequestLayout();
        }


        private static void ApplyColorToToolbarHierarchy(global::Android.Views.View toolbar, global::Android.Graphics.Color chromeColor)
        {
            global::Android.Views.View? current = toolbar;

            while (current is not null)
            {
                current.SetBackgroundColor(chromeColor);

                if (current.Parent is not global::Android.Views.View parentView)
                    break;

                var parentTypeName = parentView.GetType().Name;
                current = parentView;

                if (parentTypeName.Contains("CoordinatorLayout", StringComparison.Ordinal)
                    || parentTypeName.Contains("AppBarLayout", StringComparison.Ordinal)
                    || parentTypeName.Contains("Toolbar", StringComparison.Ordinal))
                {
                    continue;
                }

                break;
            }
        }

        private static global::Android.Views.View? FindDrawerContentView(AndroidX.DrawerLayout.Widget.DrawerLayout drawerLayout)
        {
            for (var i = 0; i < drawerLayout.ChildCount; i++)
            {
                var child = drawerLayout.GetChildAt(i);
                if (child is null)
                    continue;

                if (child.LayoutParameters is AndroidX.DrawerLayout.Widget.DrawerLayout.LayoutParams layoutParams)
                {
                    var absoluteGravity = GravityCompat.GetAbsoluteGravity(layoutParams.Gravity, (int)child.LayoutDirection);
                    if ((absoluteGravity & (int)GravityFlags.Left) == (int)GravityFlags.Left
                        || (absoluteGravity & (int)GravityFlags.Right) == (int)GravityFlags.Right
                        || (absoluteGravity & (int)GravityFlags.Start) == (int)GravityFlags.Start
                        || (absoluteGravity & (int)GravityFlags.End) == (int)GravityFlags.End)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private static AndroidX.DrawerLayout.Widget.DrawerLayout? FindDrawerLayout(global::Android.Views.View? view)
        {
            if (view is null)
                return null;

            if (view is AndroidX.DrawerLayout.Widget.DrawerLayout drawerLayout)
                return drawerLayout;

            if (view is ViewGroup group)
            {
                for (var i = 0; i < group.ChildCount; i++)
                {
                    var match = FindDrawerLayout(group.GetChildAt(i));
                    if (match is not null)
                        return match;
                }
            }

            return null;
        }

        private int GetStatusBarInset()
        {
            var decorView = Window?.DecorView;
            if (decorView is null)
                return 0;

            var insets = ViewCompat.GetRootWindowInsets(decorView);
            if (insets is not null)
            {
                var statusBarInsets = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());
                var topInset = statusBarInsets?.Top ?? 0;
                if (topInset > 0)
                    return topInset;
            }

            var resourceId = Resources?.GetIdentifier("status_bar_height", "dimen", "android") ?? 0;
            return resourceId > 0 ? Resources?.GetDimensionPixelSize(resourceId) ?? 0 : 0;
        }
        private static global::Android.Graphics.Color ResolveChromeColor(Page? page)
        {

            return Color.FromArgb("#04273F").ToPlatform();
        }

        private int GetDefaultToolbarHeight()
        {
            var density = Resources?.DisplayMetrics?.Density ?? 1f;
            return (int)Math.Round(56 * density);
        }

        private static bool IsLightColor(global::Android.Graphics.Color color)
        {
            if (color.A == 0)
                return false;

            var luminance = ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255d;
            return luminance >= 0.6d;
        }

        private void ApplyPageInsets(Page? page)
        {
            if (page is null)
                return;

            var bottomInset = GetBottomContentInset();
            _lastAppliedNavigationBarInset = bottomInset;
            var density = Resources?.DisplayMetrics?.Density ?? 1f;
            var bottomInsetDip = bottomInset / density;

            if (page.Dispatcher.IsDispatchRequired)
            {
                //page.Dispatcher.Dispatch(() => Pulse_MAUI.Views.PageLayoutInsets.ApplyBottomInset(page, bottomInsetDip));
            }
            else
            {
                //Pulse_MAUI.Views.PageLayoutInsets.ApplyBottomInset(page, bottomInsetDip);
            }
        }

        private int GetBottomContentInset()
        {
            var decorView = Window?.DecorView;
            if (decorView is null)
                return 0;

            var insets = ViewCompat.GetRootWindowInsets(decorView);
            if (insets is null)
                return 0;

            var imeInsets = insets.GetInsets(WindowInsetsCompat.Type.Ime());
            var imeBottomInset = imeInsets?.Bottom ?? 0;
            var isImeVisible = insets.IsVisible(WindowInsetsCompat.Type.Ime());

            if (isImeVisible && imeBottomInset > 0)
                return imeBottomInset;

            return 0;
        }

        private static AndroidX.AppCompat.Widget.Toolbar? FindToolbar(global::Android.Views.View? view)
        {
            if (view is null)
                return null;

            if (view is AndroidX.AppCompat.Widget.Toolbar toolbar)
                return toolbar;

            if (view is ViewGroup group)
            {
                for (var i = 0; i < group.ChildCount; i++)
                {
                    var match = FindToolbar(group.GetChildAt(i));
                    if (match is not null)
                        return match;
                }
            }

            return null;
        }

        private void EnsureDrawerListener(AndroidX.DrawerLayout.Widget.DrawerLayout drawerLayout)
        {
            if (_drawerListenerInstalled)
                return;

            drawerLayout.AddDrawerListener(new FlyoutDrawerListener(this));
            _drawerListenerInstalled = true;
        }

        public void ScheduleSystemBarRefresh(Page? page = null)
        {
            if (Window?.DecorView is null)
                return;

            _pendingRefreshPage = page ?? Shell.Current?.CurrentPage;
            if (_systemBarRefreshPending)
                return;

            _systemBarRefreshPending = true;
            Window.DecorView.Post(() =>
            {
                _systemBarRefreshPending = false;
                var targetPage = _pendingRefreshPage ?? Shell.Current?.CurrentPage;
                _pendingRefreshPage = null;
                ApplySystemBars(targetPage);
                ApplyPageInsets(targetPage);
            });
        }

        private void UpdateFlyoutStatusBarOverlay(AndroidX.DrawerLayout.Widget.DrawerLayout drawerLayout, global::Android.Views.View? flyoutView, int statusBarInset)
        {
            if (Window?.DecorView is not ViewGroup decorRoot)
                return;

            if (statusBarInset <= 0 || flyoutView is null)
            {
                if (_flyoutStatusBarOverlay is not null)
                    _flyoutStatusBarOverlay.Visibility = ViewStates.Gone;
                return;
            }

            if (_flyoutStatusBarOverlay is null || _flyoutStatusBarOverlay.Parent is null)
            {
                _flyoutStatusBarOverlay = new global::Android.Views.View(this)
                {
                    Clickable = false,
                    Focusable = false,
                    Visibility = ViewStates.Gone
                };

                var layoutParams = new global::Android.Widget.FrameLayout.LayoutParams(
                    0,
                    statusBarInset)
                {
                    Gravity = GravityFlags.Top | GravityFlags.Start
                };

                decorRoot.AddView(_flyoutStatusBarOverlay, decorRoot.ChildCount, layoutParams);
            }

            if (_flyoutStatusBarOverlay.LayoutParameters is global::Android.Widget.FrameLayout.LayoutParams overlayLayoutParams)
            {
                var overlayWidth = flyoutView.Width > 0 ? flyoutView.Width : drawerLayout.Width;
                var needsUpdate = false;

                if (overlayLayoutParams.Height != statusBarInset)
                {
                    overlayLayoutParams.Height = statusBarInset;
                    needsUpdate = true;
                }

                if (overlayLayoutParams.Width != overlayWidth && overlayWidth > 0)
                {
                    overlayLayoutParams.Width = overlayWidth;
                    needsUpdate = true;
                }

                if (needsUpdate)
                    _flyoutStatusBarOverlay.LayoutParameters = overlayLayoutParams;
            }

            _flyoutStatusBarOverlay.SetBackgroundColor(global::Android.Graphics.Color.ParseColor("#04273F"));
            var isDrawerOpen = drawerLayout.IsDrawerOpen(flyoutView);
            _flyoutStatusBarOverlay.Visibility = isDrawerOpen ? ViewStates.Visible : ViewStates.Gone;

            if (isDrawerOpen)
                _flyoutStatusBarOverlay.BringToFront();
        }


        private sealed class FlyoutDrawerListener : Java.Lang.Object, AndroidX.DrawerLayout.Widget.DrawerLayout.IDrawerListener
        {
            private readonly MainActivity _activity;

            public FlyoutDrawerListener(MainActivity activity)
            {
                _activity = activity;
            }

            public void OnDrawerClosed(global::Android.Views.View drawerView)
            {
                _activity.ScheduleSystemBarRefresh();
            }

            public void OnDrawerOpened(global::Android.Views.View drawerView)
            {
                _activity.ScheduleSystemBarRefresh();
            }

            public void OnDrawerSlide(global::Android.Views.View drawerView, float slideOffset)
            {
                if (_activity._drawerLayout is not null)
                    _activity.UpdateFlyoutStatusBarOverlay(_activity._drawerLayout, drawerView, _activity.GetStatusBarInset());
            }

            public void OnDrawerStateChanged(int newState)
            {
            }
        }
        private sealed class WindowInsetsListener : Java.Lang.Object, AndroidX.Core.View.IOnApplyWindowInsetsListener
        {
            private readonly MainActivity _activity;

            public WindowInsetsListener(MainActivity activity)
            {
                _activity = activity;
            }

            public WindowInsetsCompat? OnApplyWindowInsets(global::Android.Views.View? v, WindowInsetsCompat? insets)
            {
                _activity.OnWindowInsetsChanged();
                return insets;
            }
        }

    }
}
