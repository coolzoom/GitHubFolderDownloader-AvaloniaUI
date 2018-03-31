using System.Reflection;
using System.Diagnostics;
using Avalonia.Reactive;
using ReactiveUI;
using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace GitHubFolderDownloader.Toolkit
{
    public class TraceListenerRedirector : TraceListener
    {
        public Action<string> WriteAction;
        public override void Write(string message)
        {
            WriteAction?.Invoke(message);
        }
        public override void WriteLine(string message)
        {
            Write(message);
        }
    }
}