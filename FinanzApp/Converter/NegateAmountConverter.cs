using System.Globalization;
using System.Windows.Data;

namespace FinanzApp.Converter
{
    public class NegateAmountConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal val)
                return (-val).ToString("C", culture);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
