using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GitHubFolderDownloader.ViewModels;

namespace GitHubFolderDownloader
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}