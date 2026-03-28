using System;
using System.Text;

namespace Pulse_MAUI.Services.Navigation;

public class NavigationService : INavigationService
{
    public Task NavigateToPage<TPage>(bool isRootPage = false, IDictionary<string, object>? parameters = null)
    {
        var route = typeof(TPage).Name;
        route = isRootPage ? $"//{route}" : route;

        return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
    }

    public Task NavigateBack(int depth = 0, IDictionary<string, object>? parameters = null)
    {
        StringBuilder routeBuilder = new StringBuilder("..");
        for (int i = 0; i < depth; i++)
        {
            routeBuilder.Append("/..");
        }

        string route = routeBuilder.ToString();

        return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
    }

    public Task NavigateBackToPage(Type sourcePageType, IDictionary<string, object>? parameters = null)
    {
        var navStack = Shell.Current.Navigation.NavigationStack;

        int stepBack = navStack
            .Reverse()
            .TakeWhile(page => page.GetType() != sourcePageType)
            .Count();

        if (stepBack == navStack.Count)
            return Task.CompletedTask;

        if (stepBack == 0)
            return Task.CompletedTask;

        string route = string.Join("/", Enumerable.Repeat("..", stepBack));
        return parameters != null ? Shell.Current.GoToAsync(route, parameters) : Shell.Current.GoToAsync(route);
    }

}
