using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels.DB
{
    public class ViewModelLoader
    {
        ViewModelLoader() { }

        public void StartFetcher()
        {
            var context = new AppDBContext();
        }
    }
}
