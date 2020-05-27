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
    abstract public class Fetcher
    {
        private TimeSpan interval;
        public Fetcher()
        {
            this.interval = new TimeSpan(0, 0, 5);
        }
        [XmlIgnore]
        public TimeSpan Interval {get => interval; set => interval = value; }

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
    }
}
