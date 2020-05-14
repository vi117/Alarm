using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;

namespace Publisher {
	public class AtomFetcher : Fetcher {
		private string url;

		public AtomFetcher(string url) {
			this.url = url;
		}

		public override Task<List<Document>> Fetch() {
			var reader = XmlReader.Create(url);
			var feed = SyndicationFeed.Load(reader);
			reader.Close();

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
