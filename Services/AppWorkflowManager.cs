using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class AppWorkflowManager : IAppWorkflowManager
    {
        public AppWorkflowManager(IBlobStorageService blobStorageService,
            IEngineerService engineerService,
            IItemService itemService,
            ILookupService lookupService,
            IPunchService punchService,
            ISyncLogService syncLogService,
            ISynchroniseService synchroniseService,
            IUserService userService)
        {
            BlobStorageService = blobStorageService;
            EngineerService = engineerService;
            ItemService = itemService;
            LookupService = lookupService;
            PunchService = punchService;
            SyncLogService = syncLogService;
            SynchroniseService = synchroniseService;
            UserService = userService;
        }

        public IBlobStorageService BlobStorageService { get; }
        public IEngineerService EngineerService { get; }
        public IItemService ItemService { get; }
        public ILookupService LookupService { get; }
        public IPunchService PunchService { get; }
        public ISyncLogService SyncLogService { get; }
        public ISynchroniseService SynchroniseService { get; }
        public IUserService UserService { get; }

    }
}
