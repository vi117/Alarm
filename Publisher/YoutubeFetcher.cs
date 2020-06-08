using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Model 
{
    // Wrapper of RSSFetcher
    [Serializable]
    public class YoutubeFetcher : Fetcher
    {
        private RSSFetcher inner = null;

		public string channelId {
			get => channelId;
			set {
				var url = "https://www.youtube.com/feeds/videos.xml?channel_id=" + id;
				this.channelId = id;
				this.inner.URL =  url;
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
				channelId = id,
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

		public void SetChannelID(string id) {
			var url = "https://www.youtube.com/feeds/videos.xml?channel_id=" + id;
			this.channelId = id;
			this.inner.URL =  url;
		}
    }
}
