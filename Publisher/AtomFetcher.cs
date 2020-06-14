using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.IO;
using System;

namespace Model {
	public class AtomFetcher : Fetcher {
		private string uri;

		//serialize
		public AtomFetcher() { }

		public AtomFetcher(string uri) {
			this.uri = uri;
		}

		public string Uri { get => uri; set => uri = value; }

		public override Task<PublishedEventArg> Fetch() {
			try
			{
				var reader = XmlReader.Create(uri);
				var feed = SyndicationFeed.Load(reader); // use SyndicationFeed to load atom feed
				reader.Close();

				// Build document list
				var docs = from item in feed.Items
						   let title = item.Title.Text
						   let summary = item.Summary?.Text ?? ""
						   let id = item.Id
						   let date = item.PublishDate.ToString()
						   let url = item.Links.First()?.Uri?.ToString() ?? "about:blank"
						   select DocumentBuilder.Doc()
							   .Title(title)
							   .Summary(summary)
							   .GUID(id)
							   .pubDate(date)
							   .URL(url)
							   .Build();

				return Task.FromResult(new PublishedEventArg(docs));
			}
			catch(ArgumentException e)
            {
				return Task.FromResult(new PublishedEventArg(PublishedStatusCode.InvaildArgumentError, e.Message));
			}
			catch(UriFormatException e)
            {
				return Task.FromResult(new PublishedEventArg(PublishedStatusCode.InvaildFormatError, e.Message));
            }
			catch(FileNotFoundException e)
            {
				return Task.FromResult(new PublishedEventArg(PublishedStatusCode.ConnectionFailError, e.Message));
            }
            catch (System.Net.WebException e)
            {
				return Task.FromResult(new PublishedEventArg(PublishedStatusCode.ConnectionFailError, e.Message));
            }
			catch(Exception e)
            {
				return Task.FromResult(new PublishedEventArg(PublishedStatusCode.UnknownError, e.Message));
			}
		}
	}
}
