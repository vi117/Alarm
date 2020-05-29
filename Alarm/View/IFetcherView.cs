using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.View
{
    public interface IFetcherView
    {
        Model.Fetcher CreateFetcher();
    }
}
