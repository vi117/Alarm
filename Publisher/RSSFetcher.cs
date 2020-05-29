using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace Model
{
    [Serializable]
    public class RSSFetcher : Fetcher
    {
        private string url;
        public string URL { get => url; set => url = value; }
        //xml serialize 용
        public RSSFetcher() : base() { }
        public RSSFetcher(string url) : base() => this.url = url;
        public override Task<List<PubDocument>> Fetch()
        {
            var cur_doc = GetRSS();
            return Task.FromResult(cur_doc);
        }
        public List<PubDocument> GetRSS()
        {
            try
            {
                var root = XElement.Load(url);

                if (root.Name != "rss")
                {
                    //Error
                }
                var items = root.Elements("channel").Descendants("item");
                var doclist = from item in items
                              let title = item.Element("title").Value
                              let description = (item.Element("description")?.Value) ?? throw new Exception()
                              let guid = (item.Element("guid")?.Value) ?? title
                              let pubData = item.Element("pubDate")?.Value
                              let wholeUri = item.Element("link")?.Value ?? throw new Exception()
                              select DocumentBuilder.Doc()
                                .Title(title)
                                .Summary(description)
                                .GUID(guid)
                                .pubDate(pubData)
                                .URL(wholeUri)
                                .Build();
                return doclist.ToList();
            }
            catch(XmlException e)
            {
                Trace.WriteLine(e.Message);
                return new List<PubDocument>();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return new List<PubDocument>();
            }
        }
    }
}
