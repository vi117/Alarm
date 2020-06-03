using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Alarm.View
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || value == null) return false;

            if (parameter.GetType().IsEnum && (value is int || value.GetType() == parameter.GetType()))
            {
                return (int)parameter == (int)value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
        public static readonly EnumToBooleanConverter Instance = new EnumToBooleanConverter();
    }
}
