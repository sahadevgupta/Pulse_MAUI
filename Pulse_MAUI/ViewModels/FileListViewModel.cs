using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Popups;

namespace Pulse_MAUI.ViewModels;

public partial class FileListViewModel(IViewModelParameters viewModelParameters,
    IFileService fileService,
    ILookupService lookupService,
    IItemService itemService,
    IMediaService mediaService,
    IPopupNavigation popupNavigation) : BaseViewModel(viewModelParameters)
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

    [RelayCommand]
    private async Task DeleteImage(ImageFile imageFile)
    {
        var result = await fileService.DeleteImageAsync(imageFile, fileType == FileType.Activity ? activity.PCAId : punchItem.PunchId);
        if (result)
        {
            Files.Remove(imageFile);
        }
    }

    [RelayCommand]
    private async Task EditImageDescription(ImageFile imageFile)
    {
        var descriptionPopup = new EditImageDescriptionPopup(
            fileType == FileType.Activity ? activity.TaskCount : 0,
            imageFile.Description,
            imageFile.ChecklistStep
        );
        descriptionPopup.OkClicked += async (s, arg) =>
        {
            await UpdateImageDescriptionAsync(imageFile, arg);

        };
        await popupNavigation.PushAsync(descriptionPopup);
    }

    private async Task UpdateImageDescriptionAsync(ImageFile imageFile, Controls.CustomDialogs.DetailInputResult arg)
    {
        var response = await fileService.UpdateImageDescription(
            imageFile,
            fileType == FileType.Activity ? activity.PCAId : punchItem.PunchId,
            arg.Description,
            arg.Step
        );

        if (response)
        {
            imageFile.Description = arg.Description;
            imageFile.ChecklistStep = arg.Step != "None" ? Convert.ToInt32(arg.Step) : null;
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
