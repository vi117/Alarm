using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Publisher;

namespace PublisherTest
{
    [TestClass]
    public class AtomTest
    {
        [TestMethod]
        public void FetchTest()
        {
			// TODO: Change this test to be more sensible.
			var atom_fetcher = new AtomFetcher("https://xkcd.com/atom.xml");
			
			var title = "Common Cold";
			var id = "https://xkcd.com/2306/";

			var docs = atom_fetcher.Fetch();
			Document first_doc = docs.Result.First();

			Console.WriteLine(first_doc.Title);
			Console.WriteLine(title);
			Console.WriteLine(first_doc.GUID);
			Console.WriteLine(id);

			Assert.AreEqual(first_doc.Title, title);
			Assert.AreEqual(first_doc.GUID, id);
        }
    }
}
