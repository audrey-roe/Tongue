using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tongue
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Hide the default title bar
            ExtendsContentIntoTitleBar = true;

            SetTitleBar(AppTitleBar);

            mainFrame.Navigate(typeof(MainPage));
            Activated += MainWindow_Activated;

        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                    (Microsoft.UI.Xaml.Media.SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
            }
            else
            {
                AppTitleTextBlock.Foreground =
                    (Microsoft.UI.Xaml.Media.SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
            }
        }
    }
}
