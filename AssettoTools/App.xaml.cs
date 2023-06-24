using AssettoTools.Core.Interfaces;
using AssettoTools.Core.Services;
using AssettoTools.Core.Services.Config;
using AssettoTools.ViewModels;
using AssettoTools.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms.Design;

namespace AssettoTools
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();

                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton<IConfigReader, ConfigReader>();
                    services.AddSingleton<IFileExplorer, FileExplorer>();
                    services.AddSingleton<ICarExplorer, CarExplorer>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
