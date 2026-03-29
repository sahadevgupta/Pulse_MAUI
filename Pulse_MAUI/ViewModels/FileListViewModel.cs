using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.ViewModels;

public partial class FileListViewModel(IViewModelParameters viewModelParameters,
    IFileService fileService,
    ILookupService lookupService,
    IItemService itemService,
    IMediaService mediaService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    private FileType fileType;
    private Activity? activity;
    private PunchItem? punchItem;

    [ObservableProperty]
    private ObservableCollection<ImageFile> _files = new();

    [ObservableProperty]
    private ImageSource _imageSrc;

    #endregion


    #region [ Methods & Service Calls ]

    private void InitializeData(FileType fileType)
    {
        DialogService.ShowLoading();
        Task.Run(async () =>
        {
            try
            {
                switch (fileType)
                {
                    case FileType.Activity:
                        await FetchActivityFilesCommand.ExecuteAsync(null);
                        break;
                    case FileType.Punch:
                        await FetchPunchFilesCommand.ExecuteAsync(null);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DialogService.HideLoading();
            }
        });

    }

    public async Task<byte[]> GetImageBytesAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return null;

        using var stream = File.OpenRead(filePath);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task FetchActivityFiles()
    {
        if (activity != null)
        {
            var files = await fileService.FetchItemFiles(activity);
            Files = new ObservableCollection<ImageFile>(files);
        }
    }

    [RelayCommand]
    private async Task FetchPunchFiles()
    {
        if (punchItem != null)
        {
            var files = await fileService.FetchItemFiles(punchItem);
            Files = new ObservableCollection<ImageFile>(files);

        }
    }

    [RelayCommand]
    private async Task TakePhoto()
    {
        var result = await mediaService.TakePhoto();
        if (result is not null && activity is not null)
        {
            if (fileType == FileType.Punch)
            {
                await SaveItemAsync(result, punchItem.PunchId.GetValueOrDefault(), punchItem.ProjectId, punchItem.MobileId);

            }
            else
            {
                await SaveItemAsync(result, activity.PCAId, activity.ProjectId.GetValueOrDefault());

            }
        }
    }

    private async Task SaveItemAsync(ImageFile result, int? recordId, int projectId, string? mobileId = null)
    {
        var controlTypes = await lookupService.GetControlTypeLookups();
        int controlTypeValue = controlTypes.FirstOrDefault(c => c.Value == (fileType == FileType.Activity ? "Activity" : "Punch"))?.LookupId ?? 0;

        ImageFile image = new ImageFile();
        image.Url = result.Url;
        image.AvailableToDelete = true;

        Item item = new Item();
        item.LocalPath = result.Url;

        if (fileType == FileType.Punch)
        {
            if (recordId != null)
            {
                item.RecordId = recordId;
            }
            else
            {
                item.LocalReferenceID = mobileId;
            }
        }
        else
        {
            item.RecordId = recordId;
        }

        item.ProjectId = projectId;
        item.Name = Helpers.FileUtility.GetFileName(result.Url);
        item.ControlType = controlTypeValue;
        item.MimeType = "image/jpeg";

        Files.Add(image);

        await itemService.SaveItem(item);
    }

    [RelayCommand]
    private async Task PickPhoto()
    {
        var result = await mediaService.PickPhotoAsync();
        if (result is not null)
        {
            if (fileType == FileType.Punch)
            {
                await SaveItemAsync(result, punchItem.PunchId.GetValueOrDefault(), punchItem.ProjectId, punchItem.MobileId);

            }
            else
            {
                await SaveItemAsync(result, activity.PCAId, activity.ProjectId.GetValueOrDefault());

            }
        }
    }


    #endregion

    #region [ Override Methods ]

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);
        if (query.ContainsKey(NavigationParamConstant.FileType))
        {
            fileType = (FileType)query[NavigationParamConstant.FileType];
            activity = (Activity)query[NavigationParamConstant.Activity];
            InitializeData(fileType);
        }
    }

    #endregion

}
