﻿using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using GitHubFolderDownloader.ViewModels;
using GitHubFolderDownloader.Views;

namespace GitHubFolderDownloader
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        } 
        
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}