using Microsoft.EntityFrameworkCore;
using Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model
{
    public class AppDBContext : DbContext
    {
        public DbSet<DBCategory> Categorys { get; set; }
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
        //protected override void OnModelCreating(ModelBuilder modelBuilder){}
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
                category.Documents.Add(new DBDocument()
                {
                    Title = "AAA",
                    Date = DateTime.Now,
                    GUID = "1123",
                    HostUri = "https://www.naver.com",
                    PathUri = "",
                    Summary = "naver"
                });
                ctx.SaveChanges();
                var fs = ctx.Categorys.ToList();
                foreach (var f in fs)
                {
                    Console.WriteLine($"{f.DBCategoryId} : {f.Title} , {(f.Documents).GetType().FullName}");
                }
            }
        }
    }

    public class DBCategory
    {
        public int DBCategoryId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<DBDocument> Documents{ get; set; } = new List<DBDocument>(); /*{ get; set; }*/
    }
    /*public class DBFetcher {
        public int DBFetcherId { get; set; }
        public string Name {get; set;}
        public string FetcherXaml;
    }*/
    public class DBDocument : IDocument
    {
        public int DBDocumentId { get; set; }
        public string Title { get; set; }
        public string HostUri { get; set; }
        public string PathUri { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string GUID { get; set; }

        public DBCategory DBCategory { get; set; }
    }

}
