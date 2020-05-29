using Alarm.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ViewModel;

namespace Alarm.View.FetcherForm
{
    ///<summary>
    ///Wrapper class for <c>UserControl</c> and <c>IFetcherView</c>
    ///</summary>
    public class FetcherFormControl : UserControl, IFetcherView
    {
        virtual public Fetcher GetFetcher() { throw new NotImplementedException(); }
        virtual public string FetcherName { 
            get { throw new NotImplementedException(); } 
            set { throw new NotImplementedException(); } 
        } 
    }
}
