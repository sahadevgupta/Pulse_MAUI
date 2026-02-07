using Microsoft.Extensions.DependencyInjection;

namespace Pulse_MAUI
{
    public partial class App : Application
    {
        static double sizeFormula;
        public static double SizeFormula
        {
            get
            {
                return sizeFormula;
            }
        }
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}