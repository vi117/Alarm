using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels.Updater
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
                    fetcherView.Documents.Add(new MockDocumentViewModel(fetcherView.Root)
                    {
                        Title = doc.Title,
                        Summary = doc.Summary,
                        Date = doc.Date,
                        HostUri = doc.HostUri,
                        PathUri = doc.PathUri,
                        GUID = doc.GUID,
                    });
                }
            }
        }
        public void RegisterPublisher(DocumentPublisher publisher)
        {
            publisher.OnPublished += OnPublished;
        }
    }
    static public class MockModelUpdaterExtension
    {
        public static void RegisterUpdater(this DocumentPublisher publisher, MockFetcherViewModelUpdater updater)
        {
            updater.RegisterPublisher(publisher);
        }
    }
}
