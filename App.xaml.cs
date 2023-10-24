using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using Tongue.Services;
using Tongue.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tongue
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ITranslationService, LibreTranslateService>();
            serviceCollection.AddSingleton<IRepositoryService, RepositoryService>();
            serviceCollection.AddTransient<MainPageViewModel>();

            Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());

            // Initialize the RepositoryService
            var repositoryService = Ioc.Default.GetRequiredService<IRepositoryService>();
            await repositoryService.InitializeAsync();

            m_window.Activate();
        }

        private Window m_window;
    }
}
