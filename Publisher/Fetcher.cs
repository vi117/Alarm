using Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace Model
{
    /// <summary>
    /// Base of all fetcher class
    /// Property setter must exist.
    /// </summary>
    [Serializable]
    abstract public class Fetcher : INotifyPublished
    {
        private TimeSpan interval;

        [XmlIgnore]
        private HashSet<string> GUIDSet;
        [XmlIgnore]
        private Timer timer;
        public Fetcher()
        {
            this.interval = new TimeSpan(0, 0, 5);
            this.GUIDSet = new HashSet<string>();
        }
        [XmlIgnore]
        public TimeSpan Interval { get => interval; set => interval = value; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("IntervalTick")]
        public long IntervalTick
        {
            get { return Interval.Ticks; }
            set { Interval = new TimeSpan(value); }
        }
        /// <summary>
        ///     Fetch the documents.
        ///     It must be not NULL!
        /// </summary>
        /// <returns>The documents fetched.It must be not NULL!</returns>
        public abstract Task<List<PubDocument>> Fetch();

        public event PublishedEventHandler OnPublished;
        public async void CallWhenPublished()
        {
            Queue<PubDocument> documents = new Queue<PubDocument>();
            var docList = await this.Fetch();
            foreach (PubDocument doc in docList)
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

            OnPublished?.Invoke(this, new PublishedEventArg(documents));
        }
        //args may be null.
        public void OnElapsed(object obj, ElapsedEventArgs args)
        {
            CallWhenPublished();
        }
        /// <summary>
        /// Timer Start.
        /// </summary>
        public void Start()
        {
            timer = new Timer
            {
                AutoReset = true,
                Interval = Interval.TotalMilliseconds
            };
            timer.Elapsed += OnElapsed;
            var t = new Task(() => { CallWhenPublished(); });
            t.Start();
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }
        public void Refresh()
        {
            var t = new Task(() => { CallWhenPublished(); });
            t.Start();
        }
    }
}
