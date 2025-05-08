using System.Globalization;
using System.Windows.Data;

namespace Downloader.Converters
{
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((bool)value) == true ? false : true;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
