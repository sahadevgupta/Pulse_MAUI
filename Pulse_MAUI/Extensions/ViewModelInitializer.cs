using CommunityToolkit.Maui;
using Pulse_MAUI.Models;
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
                .AddTransient<ActivityPageViewModel>()
                .AddTransient<ActivityListPageViewModel>()
                .AddTransient<ActivityTaskViewModel>()
                .AddTransient<FileListViewModel>()
                .AddTransient<ImportSettingsPageViewModel>()
                .AddTransient<MenuPageViewModel>()
                .AddTransient<PunchListPageViewModel>()
                .AddTransient<PunchPageViewModel>();

            return builder;
        }
    }
}
