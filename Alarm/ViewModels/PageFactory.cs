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
    public interface IPageShow
    {
        /// <summary>
        /// For debugging
        /// </summary>
        string ShowingPageName
        {
            get;
        }
        /// <summary>
        /// Show related page
        /// </summary>
        Page ShowingPage
        {
            get; set;
        }

        /// <summary>
        /// create <c>Page</c> to relate ViewModel.
        /// Do not invoke directly.
        /// Use <c>PageFactory.Gernerate</c> method.
        /// </summary>
        /// <returns>page relateive to ViewModel</returns>
        Page CreatePageShowing();
    }
    class PageFactory
    {
        public static Page Generate(IPageShow modelExpressed)
        {
            var page = modelExpressed.CreatePageShowing();
            page.DataContext = modelExpressed;
            modelExpressed.ShowingPage = page;
            return page;
        }
    }
}
