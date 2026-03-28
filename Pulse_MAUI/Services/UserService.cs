using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class UserService(IDataManager dataManager) : IUserService
    {
        public User? CurrentUser { get; set; }
        /// <summary>
        /// Log into the Azure backend system.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task LoginAsync(string mobileAzureServiceUrl)
        {
            try
            {
                await dataManager.LoginAsync(mobileAzureServiceUrl);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }


        /// <summary>
        /// Gets the azure BLOB storage string.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAzureBlobStorageString()
        {
            string connection = await dataManager.GetAzureBlobConnection();
            return connection;
        }

        /// <summary>
        /// Log out of the Azure backend system.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task LogoutAsync()
        {
            await dataManager.LogoutAsync();
        }

        /// <summary>
        /// Fetches the current logged in user details and populate the CurrentUser details.
        /// </summary>
        /// <returns>The current user.</returns>
        public async Task FetchCurrentUser()
        {
            var users = await dataManager.GetAllUsersAsync();

            if (users != null)
            {
                var availableUsers = users.ToList();

                CurrentUser = availableUsers.Count > 0 ? availableUsers[0] : null;
            }
        }
    }
}
