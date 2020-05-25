using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;

namespace Model {
	public class AtomFetcher : Fetcher {
		private string uri;

		//serialize
		public AtomFetcher() { }

		public AtomFetcher(string uri) {
			this.uri = uri;
		}

		public string Uri { get => uri; set => uri = value; }

		public override Task<List<Document>> Fetch() {
			var reader = XmlReader.Create(uri);
			var feed = SyndicationFeed.Load(reader); // use SyndicationFeed to load atom feed
			reader.Close();

			// Build document list
			var docs = from item in feed.Items
				let title = item.Title.Text
				let summary = item.Summary.Text
				let id = item.Id
				let date = item.LastUpdatedTime.ToString()
				select DocumentBuilder.Doc()
				.Title(title)
				.Summary(summary)
				.GUID(id)
				.pubDate(date)
				.Build();

			return Task.FromResult(docs.ToList());
		}

	}
}
