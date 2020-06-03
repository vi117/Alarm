using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Updater
{
    /// <summary>
    /// Updater for binding FetcherViewModel with Fetcher. 
    /// </summary>
    public class MockFetcherViewModelUpdater {
        Fetcher fetcher;
        FetcherViewModel fetcherView;
        public MockFetcherViewModelUpdater(Fetcher fetcher,FetcherViewModel fetcherView) {
            this.fetcher = fetcher;
            this.fetcherView = fetcherView;
        }
        private void OnPublished(object sender,PublishedEventArg args)
        {
            if (sender == fetcher)
            {
                foreach (var doc in args.Documents)
                {
                    fetcherView.AddDocument(doc);
                }
            }
        }
    }
}
