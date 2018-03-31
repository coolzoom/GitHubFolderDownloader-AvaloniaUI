// using Avalonia;
// using Avalonia.Controls;
// using Avalonia.Xaml.Interactivity;

// namespace GitHubFolderDownloader.Behaviors
// {
//     public class FolderDialogBehavior : Trigger<Button>
//     {
//         public static readonly DependencyProperty FolderBrowserDescriptionProperty =
//            DependencyProperty.Register("FolderBrowserDescription", typeof(string),
//            typeof(FolderBrowserDialogBehavior), null);

//         public static readonly DependencyProperty FolderBrowserDialogResultCommandProperty =
//             DependencyProperty.Register("FolderBrowserDialogResultCommand",
//             typeof(object), typeof(FolderBrowserDialogBehavior), null);

//         public string FolderBrowserDescription
//         {
//             get { return (string)GetValue(FolderBrowserDescriptionProperty); }
//             set { SetValue(FolderBrowserDescriptionProperty, value); }
//         }

//         public object FolderBrowserDialogResultCommand
//         {
//             get { return GetValue(FolderBrowserDialogResultCommandProperty); }
//             set { SetValue(FolderBrowserDialogResultCommandProperty, value); }
//         }
 
//         protected override void Invoke(object parameter)
//         {
//             var folderBrowserDialog = new OpenFolderDialog();

//             if (!string.IsNullOrEmpty(FolderBrowserDescription))
//             {
//                 folderBrowserDialog.Title = FolderBrowserDescription;
//             }

//             var result = folderBrowserDialog.ShowAsync().GetAwaiter().GetResult();
//             if (result != null)
//                 FolderBrowserDialogResultCommand = result;
//         }
//     }
// }
// }