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
            var dt = new MainWindowViewModel();
            this.DataContext = dt;
            this.Closed += delegate { dt.Exit(); };
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}