using Pulse_MAUI.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        public string ProfileName
        {
            get
            {
                return "N/A";
            }

        }

        public string CurrentDate
        {
            get
            {
                return DateTime.Now.ToString("hh:mm  dd MMMM yyyy"); ;
            }
        }

        public ProfilePageViewModel()
        {
            PopulateProfile();
        }

        private void PopulateProfile()
        {
        }
    }
}

