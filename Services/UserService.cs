using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class UserService(IDataManager dataManager)
    {
        private static UserService instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static UserService Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = new UserService();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the mobile service user.
        /// </summary>
        /// <value>
        /// The mobile service user.
        /// </value>
        //public MobileServiceUser MobileServiceUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Active Directory connection is authenticated
        /// </summary>
        /// <value>
        ///   <c>true</c> if [aad authenticated]; otherwise, <c>false</c>.
        /// </value>
        public bool AADAuthenticated { get; set; }


        /// <summary>
        /// Gets or sets the azure BLOB storage.
        /// </summary>
        /// <value>
        /// The azure BLOB storage.
        /// </value>
        public string AzureBlobStorageString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PCATablet.Core.Services.UserService"/> class.
        /// </summary>

        /// <summary>
        /// Log into the Azure backend system.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task LoginAsync()
        {
            try
            {
                //await dataManager.LoginAsync();
                AADAuthenticated = true;

            }
            catch (Exception ex)
            {
                var error = ex.Message;
                AADAuthenticated = false;
                //MobileServiceUser = null;


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
