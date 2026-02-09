using CommunityToolkit.Maui;
using Pulse_MAUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Extensions
{
    public static class ViewModelInitializer
    {
        public static MauiAppBuilder ViewModelInit(this MauiAppBuilder builder)
        {
            builder.Services
                .AddTransient<ActivityListPageViewModel>()
                .AddTransient<ImportSettingsPageViewModel>()
                .AddTransient<MenuPageViewModel>();

            return builder;
        }
    }
}
