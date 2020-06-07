using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Alarm.View
{
    public class DNGreaterThan : MarkupExtension, IValueConverter
    {
        private double operand;
        public DNGreaterThan(double d)
        {
            operand = d;
        }
        protected double Operand {
            get => operand;
            set => operand = value;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) > operand;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException($"Try to cast {targetType.Name} from boolean type.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
