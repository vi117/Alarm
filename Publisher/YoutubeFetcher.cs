using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Model 
{
    // Wrapper of RSSFetcher
    public class YoutubeFetcher : Fetcher
    {
        private RSSFetcher inner = null;

        public string URL
        {
            get => inner?.URL;
            set => inner.URL = value;
        }

        [XmlIgnore]
		public string ChannelId {
			get
            {
                var match = URLRSSPattern.Match(URL);
                return match.Success ? match.Groups[1].Value : null;
            }
            set => inner.URL = "https://www.youtube.com/feeds/videos.xml?channel_id=" + value;
        }

        public YoutubeFetcher() { inner = new RSSFetcher(); }

        private static readonly Regex URLPattern = new Regex("https?://www.youtube.com/channel/(.*)");
        private static readonly Regex URLRSSPattern = new Regex("https?://www.youtube.com/feeds/videos.xml[?]channel_id=(.*)");
        static public bool IsYoutubeURL(string s)
        {
            return URLPattern.Match(s ?? "").Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns><c>null</c> if Converting is failed else url</returns>
        static public string GetIDFromYoutubeURL(string s)
        {
            var r = URLPattern.Match(s);
            return r.Success ? r.Groups[1].Value : null;
        }
        static public string GetYoutubeURLFromID(string id)
        {
            return "https://www.youtube.com/feeds/videos.xml?channel_id=" + id;
        }

        static public YoutubeFetcher FromChannelId(string id)
        {
            var url = GetYoutubeURLFromID(id);
            return new YoutubeFetcher()
            {
				ChannelId = id
            };
        }

        static public YoutubeFetcher FromURL(string url)
        {
            var id = GetIDFromYoutubeURL(url);
            if (id == null) throw new InvalidCastException();
            return FromChannelId(id);
        }

        public override Task<PublishedEventArg> Fetch()
        {
            return inner.Fetch();
        }
    }
}
