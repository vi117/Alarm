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
    class IsNewerThanNowBefore : MarkupExtension , IValueConverter
    {
        private TimeSpan before;
        public IsNewerThanNowBefore(int Hour)
        {
            before = new TimeSpan(Hour, 0, 0);
        }
        /*public int Hour
        {
            get => before.Hours;
            set => before = new TimeSpan(Hour, before.Minutes, before.Seconds);
        }*/

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (DateTime)value;
            return v > DateTime.Now - before;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
