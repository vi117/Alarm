using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels.DB
{
    static public class ViewModelLoader
    {
        static public void StartFetcher()
        {
            var context = new AppDBContext();
            var allFetchers = context.Fetchers.ToList();
            foreach(var f in allFetchers)
            {
                var fet = f.GetFetcher();
                
            }
        }
    }
}
