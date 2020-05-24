using Microsoft.EntityFrameworkCore;
using Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Model.DB
{
    public class AppDBContext : DbContext
    {
        public DbSet<DBCategory> Categorys { get; set; }
        public DbSet<DBFetcher> Fetchers { get; set; }
        public DbSet<DBDocument> Documents { get; set; }
        private static bool _created = false;
        public AppDBContext()
        {
            if (!_created)
            {
                //Database.EnsureDeleted();
                Database.EnsureCreated();
                _created = true;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=doc.db");    
        }
        public static void Test()
        {
            using (var ctx = new AppDBContext())
            {
                ctx.SaveChanges();
                ctx.Add(new DBCategory { Title = "AAA" } );
                ctx.SaveChanges();
                var category = ctx.Categorys
                    .OrderBy(b => b.DBCategoryId)
                    .First();
                var fetcher = new DBFetcher()
                {
                    Title = "AAA"
                };
                fetcher.SetFetcher(new RSSFetcher("https://www.naver.com") { Interval = new TimeSpan(0,1,0)});
                category.Fetchers.Add(fetcher);
                ctx.SaveChanges();
                var fs = ctx.Categorys.ToList();
                foreach (var f in fs)
                {
                    Console.WriteLine($"{f.DBCategoryId} : {f.Title} , {f.Fetchers.GetType().FullName}");
                    Console.WriteLine($"{f.Fetchers.First().FetcherXml}");
                    break;
                }
            }
        }
    }

    public class DBCategory
    {
        public int DBCategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        public virtual List<DBFetcher> Fetchers{ get; set; } = new List<DBFetcher>(); /*{ get; set; }*/
    }
    public class DBFetcher {
        public int DBFetcherId { get; set; }
        [Required]
        public string Title {get; set;}
        public string FetcherXml { get; set; }
        public virtual List<DBDocument> Documents { get; set; } = new List<DBDocument>(); /*{ get; set; }*/

        public int DBCategoryId { get; set; }
        virtual public DBCategory DBCategory { get; set; }

        public void SetFetcher(Fetcher fetcher)
        {
            var ser = new XmlSerializer(typeof(Fetcher), ConcreteFetcherList.types);
            var stream = new StringWriter();
            ser.Serialize(stream, fetcher);
            FetcherXml = stream.ToString();
            stream.Close();
        }
        public Fetcher GetFetcher()
        {
            var ser = new XmlSerializer(typeof(Fetcher), ConcreteFetcherList.types);
            using (var stream = new StringReader(FetcherXml))
            {
                return ser.Deserialize(stream) as Fetcher;
            }
        }
    }
    public class DBDocument : IDocument
    {
        public int DBDocumentId { get; set; }
        [Required]
        public string Title { get; set; }
        public string HostUri { get; set; }
        public string PathUri { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string GUID { get; set; }
        
        public bool IsRead { get; set; }

        public int DBFetcherId { get; set; }
        virtual public DBFetcher DBFetcher{ get; set; }
    }

}
