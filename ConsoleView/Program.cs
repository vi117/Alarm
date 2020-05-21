using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;

namespace ConsoleView
{
    public class MockFetcher : Fetcher
    {
        public override Task<List<Document>> Fetch()
        {
            var docs = new List<Document>();
            var doc = new Document();
            doc.Title = "Mock";
            doc.Summary = "Mock";
            docs.Add(doc);
            return Task.FromResult(docs);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Model.DB.AppDBContext.Test();
            RSSFetcher obj = new RSSFetcher(@"https://media.daum.net/syndication/economic.rss");
           /* var infos = obj.GetType().GetProperties();
            foreach (var info in infos)
            {
                Console.WriteLine($"{info.CanWrite},{info.CanRead},{info.PropertyType},{info.Name}");
                object v = info.GetGetMethod().Invoke(obj,null);
                Console.WriteLine($"Value : {v}");
            }*/
            
            var q = new Queue<Document>();
            DocumentPublisher publisher = new DocumentPublisher();
            publisher.AddFetcher(new RSSFetcher(@"https://media.daum.net/syndication/economic.rss"));
            publisher.OnPublished += (o, e) => {    
                foreach(var doc in e.Documents)
                {
                    q.Enqueue(doc);
                }
            };

            //publisher.AddFetcher(new RSSFetcher(@"http://www.aving.net/rss/life.xml"));
            //error
            //publisher.AddFetcher(new RSSFetcher(@"http://www.ilemonde.com/rss/allArticle.xml"));
            while (true)
            {
                while (true)
                {
                    if (q.Count > 0)
                    {
                        var doc = q.Dequeue();
                        Console.WriteLine(doc.Title);
                    }
                    else break;
                }
                Thread.Sleep(100);
            }
            //Console.ReadLine();
        }
    }
}
