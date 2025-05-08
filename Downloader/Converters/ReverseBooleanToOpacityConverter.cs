using System.Globalization;
using System.Windows.Data;

namespace Downloader.Converters
{
    public class ReverseBooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((bool)value) == true ? 0.5 : 1;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
