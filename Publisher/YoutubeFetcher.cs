using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Model 
{
    // Wrapper of RSSFetcher
    [Serializable]
    public class YoutubeFetcher : Fetcher
    {
        private RSSFetcher inner = null;

        public string URL { 
            get => inner.URL;
            set {
                if (inner == null) inner = new RSSFetcher();
                inner.URL = value;
            }
        }
        //xml serialize 용도.
        public YoutubeFetcher() { }

        static private readonly Regex URLPattern = new Regex("https://www.youtube.com/channel/(.*)");

        static public bool IsYoutubeURL(string s)
        {
            return URLPattern.Match(s).Success;
        }

        static public YoutubeFetcher FromChannelId(string id)
        {
            var url = "https://www.youtube.com/feeds/videos.xml?channel_id=" + id;
            var inner = new RSSFetcher(url);
            return new YoutubeFetcher()
            {
                inner = inner
            };
        }

        static public YoutubeFetcher FromURL(string url)
        {
            var r = URLPattern.Match(url);
            var id = r.Captures[0].Value;

            return FromChannelId(id);
        }

        public override Task<PublishedEventArg> Fetch()
        {
            return this.inner.Fetch();
        }
    }
}
