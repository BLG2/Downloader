using System.Globalization;
using System.Windows.Data;

namespace Downloader.Converters
{
    public class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((bool)value) == true ? 1 : 0.5;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
