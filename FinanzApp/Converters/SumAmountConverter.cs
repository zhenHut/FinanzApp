using FinanzApp.core.Models;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace FinanzApp.Converters
{
    public class SumAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IEnumerable items)
                return 0m;

            decimal sum = 0m;
            foreach (var item in items)
            {
                switch (item)
                {
                    case Transaction t:
                        sum += t.Amount;
                        break;

                    default:
                        // Fallback: per Reflection nach "Amount" (decimal) suchen
                        var prop = item?.GetType().GetProperty("Amount", BindingFlags.Public | BindingFlags.Instance);
                        if (prop != null && prop.PropertyType == typeof(decimal))
                        {
                            var v = prop.GetValue(item);
                            if (v is decimal d)
                                sum += d;
                        }
                        break;

                }
            }
            return sum;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
