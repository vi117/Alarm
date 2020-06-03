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
using System.ServiceModel.Syndication;
using System.IO;

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
        public override Task<PublishedEventArg> Fetch()
        {
            return Task.FromResult(GetRSS());
        }
        public PublishedEventArg GetRSS()
        {
            try
            {
                var reader = XmlReader.Create(url);
                var root = SyndicationFeed.Load(reader);
                reader.Close();

                var items = root.Items;
                var doclist = from item in items
                              let title = item.Title.Text
                              let description = item.Summary.Text
                              let guid = item.Id
                              let pubData = item.LastUpdatedTime.DateTime
                              let wholeUri = item.Links.First().Uri.ToString()
                              select DocumentBuilder.Doc()
                                .Title(title)
                                .Summary(description)
                                .GUID(guid)
                                .pubDate(pubData)
                                .URL(wholeUri)
                                .Build();
                return new PublishedEventArg(doclist);
            }
            catch (XmlException e)
            {
                Trace.WriteLine(e.Message);
                return new PublishedEventArg(PublishedStatusCode.InvaildFormatError,"XML Format error : "+ e.Message);
            }
            catch(FileNotFoundException e)
            {
                return new PublishedEventArg(PublishedStatusCode.ConnectionFailError, "Connection failed : " + e.Message);
            }
            /*catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return new PublishedEventArg(PublishedStatusCode.UnknownError, e.Message);
            }*/
        }
    }
}
