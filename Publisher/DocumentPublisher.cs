using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Model
{
    class FetcherFilter
    {
        private Fetcher fetcher;
        private HashSet<string> GUIDSet;
        private Queue<Document> documents;
        public FetcherFilter(Fetcher fetcher,Queue<Document> documents)
        {
            this.fetcher = fetcher;
            this.documents = documents;
            this.GUIDSet = new HashSet<string>();
        }

        public async void OnElapsed(object obj, ElapsedEventArgs args)
        {
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
        }
        static public ElapsedEventHandler GetHandler(Fetcher fetcher, Queue<Document> documents)
        {
            return (new FetcherFilter(fetcher, documents)).OnElapsed;
        }
    }
    public class DocumentPublisher
    {
        private List<Timer> timers;
        private Queue<Document> documents;
        public DocumentPublisher()
        {
            timers = new List<Timer>();
            documents = new Queue<Document>();
        }
        public void AddFetcher(Fetcher fetcher)
        {
            var t = new Timer();
            t.AutoReset = true;
            t.Interval = fetcher.Interval.TotalMilliseconds;
            t.Elapsed += FetcherFilter.GetHandler(fetcher, documents);
            timers.Add(t);
        }
        public void Start()
        {
            foreach(var t in timers)
            {
                t.Start();
            }
        }
        /// <summary>
        /// Pop the subscribed document
        /// </summary>
        /// <returns>document or null if queue is empty</returns>
        public Document PopDocument()
        {
            lock (documents)
            {
                if (documents.Count != 0)
                    return documents.Dequeue();
                else return null;
            }
        }
    }
}
