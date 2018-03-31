using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GitHubFolderDownloader.Views
{
    public class Progress : UserControl
    {
        public Progress()
        {
            InitializeComponent();
        }
        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}