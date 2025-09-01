using System.Globalization;
using System.Windows.Data;

namespace FinanzApp.Converters
{
    public class DecimalInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is decimal dec)
                return dec.ToString(culture);

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value?.ToString();

            if (string.IsNullOrWhiteSpace(text))
                return 0m;

            text = text.Replace(",", culture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".", culture.NumberFormat.NumberDecimalSeparator);

            if(decimal.TryParse(text,NumberStyles.Any, culture, out var result))
                return result;

            return Binding.DoNothing;
        }
    }
}
