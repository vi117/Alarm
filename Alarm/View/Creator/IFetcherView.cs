using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.View.Creator
{
    public interface IFetcherView
    {
        Publisher.Fetcher GetFetcher();
    }
}
