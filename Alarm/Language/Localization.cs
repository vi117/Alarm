using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alarm.Language
{
    static public class Localization
    {
        public static string GetText(this FrameworkElement element, string key)
        {
            try
            {
                var v = element.FindResource(key);
                return (string)v;
            }
            catch (ResourceReferenceKeyNotFoundException _)
            {
                return "{" + key + "}";
            }
        }
    }
}
