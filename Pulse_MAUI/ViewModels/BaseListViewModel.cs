using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pulse_MAUI.Events;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Resources.Languages;

namespace Pulse_MAUI.ViewModels;

public abstract partial class BaseListViewModel : BaseViewModel
{
    #region [ Properties ]

    public IAsyncRelayCommand RefreshCommand { get; }

    [ObservableProperty]
    private ObservableCollection<string>? _commSystem;

    [ObservableProperty]
    private ObservableCollection<string>? _componentTags;

    [ObservableProperty]
    private ObservableCollection<string>? _componentTypes;

    [ObservableProperty]
    private ObservableCollection<string>? _units;

    [ObservableProperty]
    private ObservableCollection<string>? _activitiesName;

    [ObservableProperty]
    private string? _selectedActivity;

    [ObservableProperty]
    private string? _selectedUnit;

    [ObservableProperty]
    private string? _selectedCommSystem;

    [ObservableProperty]
    private string? _selectedComponentType;

    [ObservableProperty]
    private string? _selectedComponentTag;

    public BaseListViewModel(IViewModelParameters viewModelParameters) : base(viewModelParameters)
    {
        RefreshCommand = new AsyncRelayCommand(async () => await RefereshItems());
    }



    #endregion

    #region [ Methods & Service Calls ]

    protected abstract Task RefereshItems();

    partial void OnSelectedUnitChanged(string? oldValue, string? newValue)
    {
        // Derived VMs do not need to override unless they want to
        OnUnitChanged(newValue);
    }

    partial void OnSelectedCommSystemChanged(string? oldValue, string? newValue)
    {
        // Derived VMs do not need to override unless they want to
        OnCommSystemChanged(newValue);
    }

    partial void OnSelectedComponentTagChanged(string? oldValue, string? newValue)
    {
        // Derived VMs do not need to override unless they want to
        OnComponentTagChanged(newValue);
    }

    partial void OnSelectedComponentTypeChanged(string? oldValue, string? newValue)
    {
        OnComponentTypeChanged(newValue);
    }

    partial void OnSelectedActivityChanged(string? oldValue, string? newValue)
    {
        OnActivityChanged(newValue);
    }

    /// <summary>
    /// Virtual method to allow child VMs to override selected unit behavior.
    /// </summary>
    protected virtual void OnUnitChanged(string? newValue)
    {
        // Default behavior: you can add a spinner, logging, etc.
    }

    /// <summary>
    /// Virtual method to allow child VMs to override selected Comm System  behavior.
    /// </summary>
    protected virtual void OnCommSystemChanged(string? newValue)
    {
        // Default behavior: you can add a spinner, logging, etc.
    }

    /// <summary>
    /// Virtual method to allow child VMs to override selected Component Tag behavior.
    /// </summary>
    protected virtual void OnComponentTagChanged(string? newValue)
    {
        // Default behavior: you can add a spinner, logging, etc.
    }

    /// <summary>
    /// Virtual method to allow child VMs to override selected Component Type  behavior.
    /// </summary>
    protected virtual void OnComponentTypeChanged(string? newValue)
    {
        // Default behavior: you can add a spinner, logging, etc.
    }

    /// <summary>
    /// Virtual method to allow child VMs to override selected Activity  behavior.
    /// </summary>
    protected virtual void OnActivityChanged(string? newValue)
    {
        // Default behavior: you can add a spinner, logging, etc.
    }

    private async Task RegisterEvents()
    {
        WeakReferenceMessenger.Default.Register<NotificationMessageEvent>(this, async (r, m) => await OnNotificationMessageReceived());
    }

    private void DeregisterEvents()
    {
        WeakReferenceMessenger.Default.Unregister<NotificationMessageEvent>(this);
    }

    private async Task OnNotificationMessageReceived()
    {
        await RefreshCommand.ExecuteAsync(null);
    }

    protected bool HasOptionSelected(string? option)
    {
        if (!string.IsNullOrEmpty(option) && option != UserInterface.SearchView_All)
        {
            return true;
        }

        return false;
    }



    #endregion

    #region [ Override Methods ]

    public override void LoadDataOnNavigatedTo()
    {
        _ = RegisterEvents();
    }

    public override void LoadDataOnDisappearing()
    {
        DeregisterEvents();
    }

    #endregion

}
