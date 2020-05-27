using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MockFetcher : Fetcher
    {
        public MockFetcher() { }
        
        public override Task<List<PubDocument>> Fetch()
        {
            var docs = new List<PubDocument>();
            var doc = new PubDocument();
            doc.Title = "Mock";
            doc.Summary = "Mock";
            doc.Uri = "http://127.0.0.1";
            doc.GUID = "5";
            doc.Date = DateTime.Now;
            docs.Add(doc);
            return Task.FromResult(docs);
        }
    }
}
