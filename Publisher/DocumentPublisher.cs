using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Timers;

namespace Model
{
    class FetcherFilter
    {
        private Fetcher fetcher;
        private HashSet<string> GUIDSet;

        public event PublishedEventHandler OnPublished;

        public FetcherFilter(Fetcher fetcher)
        {
            this.fetcher = fetcher;
            this.GUIDSet = new HashSet<string>();
        }

        public async void OnElapsed(object obj, ElapsedEventArgs args)
        {
            Queue<Document> documents = new Queue<Document>();
            var docList = await fetcher.Fetch();
            foreach (Document doc in docList)
            {
                if (!GUIDSet.Contains(doc.GUID))
                {
                    lock (documents)
                    {
                        documents.Enqueue(doc);
                    }
                    GUIDSet.Add(doc.GUID);
                }
            }
            
            OnPublished?.Invoke(fetcher, new PublishedEventArg(documents));
        }
        static public ElapsedEventHandler GetHandler(Fetcher fetcher, 
            PublishedEventHandler publishedEventHandler)
        {
            var ret = new FetcherFilter(fetcher);
            ret.OnPublished += publishedEventHandler;
            return ret.OnElapsed;
        }
    }
    public class PublishedEventArg : EventArgs {
        private Queue<Document> documents;

        public PublishedEventArg(Queue<Document> documents)
        {
            Documents = documents;
        }

        public Queue<Document> Documents { get => documents; set => documents = value; }
    }
    public delegate void PublishedEventHandler(object sender, PublishedEventArg arg);

    public class DocumentPublisher
    {
        private List<Timer> timers;

        public event PublishedEventHandler OnPublished;

        public DocumentPublisher()
        {
            timers = new List<Timer>();
            //OnPublished += (o, e) => { };
        }
        public void AddFetcher(Fetcher fetcher)
        {
            var t = new Timer
            {
                AutoReset = true,
                Interval = fetcher.Interval.TotalMilliseconds
            };
            t.Elapsed += FetcherFilter.GetHandler(fetcher, (o,e)=> { OnPublished?.Invoke(o, e); });
            timers.Add(t);
            t.Start();
        }
        /*public void Start()
        {
            foreach(var t in timers)
            {
                t.Start();
            }
        }*/
        /// <summary>
        /// refresh timers.
        /// </summary>
        public void Refresh()
        {
            foreach(var t in timers)
            {
                t.Stop();
                t.Start();
            }
        }
    }
}
