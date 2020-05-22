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
        public RSSFetcher() { }
        public RSSFetcher(string url) : base() => this.url = url;
        public override Task<List<Document>> Fetch()
        {
            var cur_doc = GetRSS();
            return Task.FromResult(cur_doc);
        }
        public List<Document> GetRSS()
        {
            try
            {
                var root = XElement.Load(url);
                var items = root.Elements("channel").Descendants("item");
                var doclist = from item in items
                              let title = item.Element("title").Value
                              let description = item.Element("description").Value
                              let guid = (item.Element("guid")?.Value) ?? title
                              let pubData = item.Element("pubDate").Value
                              let wholeUri = item.Element("link").Value
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
                return new List<Document>();
            }
        }
    }
}
