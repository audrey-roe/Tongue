using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Tongue.Models;
using Tongue.Services;
using Tongue.ViewModels;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tongue
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; } = new();
        
        private DispatcherTimer _timer;
        private bool _shouldTrigger;
        private ITranslationService service;

        public MainPage()
        {
            this.InitializeComponent();
            // SetCustomTitleBar();
            service = new LibreTranslateService();
            _timer = new()
            {
                Interval = TimeSpan.FromMilliseconds(1500)
            };

            _timer.Tick += OnTimerTick;
            _timer.Start();

            ViewModel.GetTranslationHistoryCommand?.Execute(null);

            OnTranslationHistoryCollectionChanged(null, null);
            ViewModel.TranslationHistory.CollectionChanged += OnTranslationHistoryCollectionChanged;

            Unloaded += OnMainPageUnloaded;
            Loaded += MainPage_Loaded;

        }
        private void SetCustomTitleBar()
        {
            // Get the title bar of the current view
            // ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Define the custom title bar colors and buttons
            // titleBar.BackgroundColor = Windows.UI.Colors.YourBackgroundColor;
            // titleBar.ButtonBackgroundColor = Windows.UI.Colors.YourButtonBackgroundColor;
            // titleBar.ButtonForegroundColor = Windows.UI.Colors.YourButtonForegroundColor;

            // Hide the default system title bar
            // CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            // Set your custom title bar content here
            // For example, you can create XAML elements for the title and buttons
            // and add them to your title bar grid.

            // For the icon, you can use an Image or SymbolIcon control and add it to your custom title bar.
        }

        // Handle the Loaded event
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Access the service provider and resolve the ITranslationService
            service = Ioc.Default.GetRequiredService<ITranslationService>();
        }

        private void OnTranslationHistoryCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!ViewModel.TranslationHistory.Any())
                VisualStateManager.GoToState(this, "NoHistoryState", false);
            else
                VisualStateManager.GoToState(this, "HistoryAvailableState", false);
        }

        [RelayCommand]
        private void Copy(bool isSource = false)
        {
            var dataPackage = new DataPackage()
            {
                RequestedOperation = DataPackageOperation.Copy
            };

            dataPackage.SetText(isSource ? ViewModel.SourceText : ViewModel.TranslatedText);

            Clipboard.SetContent(dataPackage);
        }

        private void OnMainPageUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= OnTimerTick;
        }

        private async void OnTimerTick(object sender, object e)
        {
            var oldText = ViewModel.SourceText;
            ViewModel.SourceText = SourceTextBox.Text;

            if (!string.IsNullOrEmpty(ViewModel.SourceText)
                && oldText != ViewModel.SourceText
                && _shouldTrigger)
            {
                _shouldTrigger = false;
                ViewModel.SourceCharCount = ViewModel.SourceText.Length;
                await ViewModel.TranslateCommand?.ExecuteAsync(false);
                ViewModel.TranslationCharCount = ViewModel.TranslatedText.Length;
            }
        }

        private void OnSourceTextBoxTextChanged(object sender, TextChangedEventArgs args)
        {
            _shouldTrigger = true;
        }

        private async void OnSourceComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            ViewModel.SelectedSourceLangInfo = (LanguageInfo)comboBox.SelectedItem;

            await ViewModel.TranslateCommand?.ExecuteAsync(false);
        }

        private async void OnTranslationComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            ViewModel.SelectedTranslationLangInfo = (LanguageInfo)comboBox.SelectedItem;

            await ViewModel.TranslateCommand?.ExecuteAsync(false);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (TranslationHistory)((FrameworkElement)e.OriginalSource).DataContext;

            ViewModel.RemoveHistoryItemCommand?.Execute(item);
        }

    }
}
