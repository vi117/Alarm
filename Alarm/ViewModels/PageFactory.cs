using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm.ViewModels
{
    public interface IAlertPage
    {
        string ValidPageName
        {
            get;
        }
    }
    class PageFactory
    {
        public static Page Generate(string name)
        {
            switch (name)
            {
                case "CategoryView":
                    return new CategoryView();
                case "ContentListView":
                    return new ContentListView();
                case "ContentView":
                    return new ContentView();
                default:
                    return new EmptyPage(name);
            }
        }
        public static Page Generate(IAlertPage page)
        {
            var ret = Generate(page.ValidPageName);
            ret.DataContext = page;
            return ret;
        }
    }
}
