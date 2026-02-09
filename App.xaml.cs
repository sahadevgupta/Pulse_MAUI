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

        public static void SetSizeFormula(double topMultiplier, float xdpi)
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                sizeFormula = 1;
            }
            else
            {

                if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
                {
                    topMultiplier = topMultiplier * 1.5;
                }

                sizeFormula = topMultiplier / (xdpi / 428);

            }
        }
    }
}