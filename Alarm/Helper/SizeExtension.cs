using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.Helper
{
    static class SizeExtension
    {
        public static bool IsInRelative(this System.Windows.Size size,System.Windows.Point point)
        {
            return 0 <= point.X && point.X <= size.Width
                && 0 <= point.Y && point.Y <= size.Height;
        }
    }
}
