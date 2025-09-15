using System.Globalization;
using System.Windows.Data;

namespace FinanzApp.Converters
{
    public class NullToLabelConverter : IValueConverter
    {
        #region Property

        public string EmptyLabel { get; set; } = "Ohne Kategorie";
        #endregion

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            return string.IsNullOrWhiteSpace(s) ? EmptyLabel : s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
        #endregion
    }
}
