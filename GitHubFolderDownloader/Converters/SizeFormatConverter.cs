using System;
using Avalonia;
using Avalonia.Markup;
using System.Globalization;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.Converters
{
    public class SizeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var size = (long)value;
            return FilesInfo.FormatSize(size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
