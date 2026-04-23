using Mopups.Pages;
using System.Collections.ObjectModel;
using Pulse_MAUI.Models;
using Mopups.Services;

namespace Pulse_MAUI.Popups
{
    public partial class SyncProgressPopup : PopupPage
    {
        public ObservableCollection<SyncStepModel> SyncSteps { get; set; } = new();


        private string _currentStep;
        public string CurrentStep
        {
            get => _currentStep;
            set
            {
                _currentStep = value;
                currentSteplbl.Text = value;
            }

        }

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                button.Text = value;
            }

        }
        public SyncProgressPopup(ObservableCollection<SyncStepModel> steps, string currentStep)
        {
            InitializeComponent();
            SyncSteps = steps;
            CurrentStep = currentStep;
            BindingContext = this;
        }

        private void button_Clicked(object sender, EventArgs e)
        {
            if (button.Text == "Done")
            {
                MopupService.Instance.PopAsync();
            }
        }
    }
}
