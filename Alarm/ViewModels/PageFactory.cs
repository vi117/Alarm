﻿using System;
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
using ViewModel;
namespace Alarm.ViewModels.Interface { }
namespace Alarm.ViewModels
{
    class PageFactory : ViewModel.IPageFactory
    {
        //content view 는 무거워서 재활용한다. 싱글톤임.
        static View.ContentView contentView = null;


        public object Generate(IPageShow modelExpressed)
        {
            var pageName = modelExpressed.ShowingPageName;
            Page page = null;
            switch (pageName) {
                case "ContentView":
                    if (contentView == null) contentView = new View.ContentView();
                    page = contentView;
                    break;
                case "ContentListView":
                    page = new View.ContentListView();
                    break;
                case "CategoryView":
                    page = new View.CategoryView();
                    break;
                default:
                    page = new View.EmptyPage();
                    break;
            }
            page.DataContext = modelExpressed;
            modelExpressed.ShowingPage = page;
            return page;
        }
        public static PageFactory Factory { get; } = new PageFactory();
    }
}
