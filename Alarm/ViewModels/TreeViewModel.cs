using Microsoft.EntityFrameworkCore;
using Publisher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm
{
    public interface IAlertPage
    {
        string ValidPageName
        {
            get;
        }
    }
    public class DocumentDbContext : DbContext{
        public DocumentDbContext() : base() { }
        public DbSet<Document> Documents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=doc.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
    [Table("Document")]
    public class DocumentView : ViewModelBase, IAlertPage, IDocument
    {
        public string Title { get; set;}

        public string HostUri { get; set; }
        public string PathUri { get; set; }
        public string Uri
        {
            get => HostUri + "/" + PathUri;
            set
            {
                var uri = new Uri(value);
                HostUri = uri.Host;
                PathUri = uri.PathAndQuery;
            }
        }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string GUID { get; set; }

        public DocumentView():base()
        {}
        public DocumentView(IDocument document):base()
        {
            HostUri = document.HostUri;
            PathUri = document.PathUri;
            Date = document.Date;
            GUID = document.GUID;
            Summary = document.Summary;
            Title = document.Title;
        }
        public string ValidPageName => "ContentView";
    }
    public class SiteModel : ViewModelBase, IAlertPage
    {
        private string title;
        private ObservableCollection<DocumentView> documents;
        public SiteModel()
        {
            documents = new ObservableCollection<DocumentView>();
        }
        public SiteModel(string title):this()
        {
            this.title = title;
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public ObservableCollection<DocumentView> Documents
        {
            get => documents;
            set
            {
                documents = value;
            }
        }

        public string ValidPageName => "ContentListView";

        public void RemoveFirstDocument()
        {
            documents.Remove(documents.First());
            OnPropertyChanged(nameof(Documents));
        }
        public void RemoveDocument(DocumentView document)
        {
            OnPropertyChanged(nameof(Documents));
        }
        public void Add(DocumentView document)
        {
            documents.Add(document);
            OnPropertyChanged(nameof(Documents));
        }
    }
    public class CategoryItem : ViewModelBase, IAlertPage
    {
        private string title;
        private ObservableCollection<SiteModel> siteModels;
        public CategoryItem()
        {
            this.title = "No Named Category";
        }
        public CategoryItem(string title)
        {
            this.title = title;
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public ObservableCollection<SiteModel> SiteModels
        {
            get => siteModels;
            set
            {
                siteModels = value;
                OnPropertyChanged(nameof(SiteModels));
            }
        }

        public string ValidPageName => "CategoryView";
    }
    public class TreeViewModel : ObservableCollection<CategoryItem>
    {
        public TreeViewModel():base()
        {}
    }
    class PageFactory
    {
        public static Page Generate(string name)
        {
            switch (name)
            {
                case "CategoryView":
                    return new CategoryView();
                case "ContentListView":
                    return new ContentListView();
                case "ContentView":
                    return new ContentView();
                default:
                    return new EmptyPage();
            }
        }
        public static Page Generate(IAlertPage page)
        {
            var ret = Generate(page.ValidPageName);
            ret.DataContext = page;
            return ret;
        }
    }
}
