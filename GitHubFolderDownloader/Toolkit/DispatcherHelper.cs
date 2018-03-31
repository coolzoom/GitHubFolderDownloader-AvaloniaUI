using System;
using Avalonia.Threading;

namespace GitHubFolderDownloader.Toolkit
{
    public static class DispatcherHelper
    {
        public static void DispatchAction(Action action,
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
        {

            if (action == null)
                return;

            Dispatcher.UIThread.InvokeAsync(action, dispatcherPriority);
        }
    }
}