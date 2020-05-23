using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
            this.interval = new TimeSpan(0, 0, 1);
        }
        public TimeSpan Interval {get => interval; set => interval = value; }
        /// <summary>
        ///     Fetch the documents.
        ///     It must be not NULL!
        /// </summary>
        /// <returns>The documents fetched.It must be not NULL!</returns>
        public abstract Task<List<Document>> Fetch();
    }
}
