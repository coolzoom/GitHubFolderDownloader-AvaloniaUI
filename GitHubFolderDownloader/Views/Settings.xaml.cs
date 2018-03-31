using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GitHubFolderDownloader.Views
{
    public class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }
        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}