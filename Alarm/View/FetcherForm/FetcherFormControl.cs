using Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.View.FetcherForm
{
    ///<summary>
    ///Wrapper class for <c>UserControl</c> and <c>IFetcherView</c>
    ///</summary>
    public class FetcherFormControl : UserControl, IFetcherView
    {
        virtual public Fetcher GetFetcher() { throw new NotImplementedException(); }
    }
}
