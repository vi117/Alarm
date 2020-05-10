using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels
{
    public interface IViewModelBehavior
    {
        void Navigate(IAlertPage model);
    }
}
