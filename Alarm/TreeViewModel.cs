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
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm
{
    interface IAlertPage
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
    }
    [Table("Document")]
    public class DocumentView : Document, IAlertPage
    {
        public DocumentView():base()
        {
            /*if (!System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio")){               
                throw new Exception("Called Not in Design Mode");
            }*/
        }
        public DocumentView(Document document):base()
        {
            BaseUrl = document.BaseUrl;
            Date = document.Date;
            GUID = document.GUID;
            Summary = document.Summary;
            Title = document.Title;
        }
        public string ValidPageName => "DocumentView.xaml";
    }
    public class SiteModel : INotifyPropertyChanged, IAlertPage
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
            OnPropertyChanged("Documents");
        }
        public void RemoveDocument(DocumentView document)
        {
            OnPropertyChanged("Documents");
        }
        public void Add(DocumentView document)
        {
            documents.Add(document);
            OnPropertyChanged("Documents");
        }

        private void OnPropertyChanged(string propertyName)
        {
            Trace.WriteLine($"{propertyName} is changed!");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class CategoryItem : INotifyPropertyChanged, IAlertPage
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
                OnPropertyChanged("Title");
            }
        }
        public ObservableCollection<SiteModel> SiteModels
        {
            get => siteModels;
            set
            {
                siteModels = value;
                OnPropertyChanged("SiteModels");
            }
        }

        public string ValidPageName => "CategoryView";

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class TreeViewModel : ObservableCollection<CategoryItem>
    {
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
                case "DocumentView":
                default:
                    return new EmptyPage();
            }
        }
    }
}
