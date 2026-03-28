using System;

namespace Pulse_MAUI.Services.Navigation;

public interface INavigationService
{
    Task NavigateBack(int depth = 0, IDictionary<string, object>? parameters = null);
    Task NavigateBackToPage(Type sourcePageType, IDictionary<string, object>? parameters = null);
    Task NavigateToPage<TPage>(bool isRootPage = false, IDictionary<string, object>? parameters = null);
}
