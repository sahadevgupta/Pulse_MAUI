using System;

namespace Pulse_MAUI.Views;

internal static class PageLayoutInsets
{
    private static readonly BindableProperty BasePaddingProperty = BindableProperty.CreateAttached(
        "BasePadding",
        typeof(Thickness?),
        typeof(PageLayoutInsets),
        null);

    public static void ApplyBottomInset(Page page, double bottomInsetDip)
    {
        if (page is not BasePage contentPage)
            return;

        var basePadding = (Thickness?)contentPage.GetValue(BasePaddingProperty);
        if (basePadding == null)
        {
            basePadding = contentPage.Padding;
            contentPage.SetValue(BasePaddingProperty, basePadding);
        }

        var currentBasePadding = basePadding.Value;
        var targetPadding = new Thickness(
            currentBasePadding.Left,
            currentBasePadding.Top,
            currentBasePadding.Right,
            currentBasePadding.Bottom + Math.Max(0, bottomInsetDip));

        if (!contentPage.Padding.Equals(targetPadding))
            contentPage.Padding = targetPadding;
    }
}
