using Publisher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Diagnostics;
using System.Windows.Input;

namespace Alarm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class ViewModel : ViewModelBase
    {
        private TreeViewModel treeView;
        private Page displayPage;
        public ViewModel()
        {
            //test data
            {
                treeView = new TreeViewModel();
                {
                    var sc = new CategoryItem("Science");
                    sc.SiteModels = new ObservableCollection<SiteModel>();
                    var siteA = new SiteModel("A");
                    {
                        var doc = new Document
                        {
                            Title = "News1",
                            Summary = "News1 Summary",
                            Date = DateTime.Now,
                            GUID = "1"
                        };
                        siteA.Add(new DocumentView(doc));
                        doc = new Document
                        {
                            Title = "News2",
                            Summary = "News2 Summary",
                            Date = DateTime.Now,
                            GUID = "2"
                        };
                        siteA.Add(new DocumentView(doc));
                    }
                    sc.SiteModels.Add(siteA);
                    sc.SiteModels.Add(new SiteModel("B"));
                    sc.SiteModels.Add(new SiteModel("C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new CategoryItem("Youtube");
                    yt.SiteModels = new ObservableCollection<SiteModel>();
                    yt.SiteModels.Add(new SiteModel("A"));
                    yt.SiteModels.Add(new SiteModel("B"));
                    treeView.Add(yt);
                }
            }
        }
        public TreeViewModel TreeView
        {
            get => treeView;
            set
            {
                treeView = value;
                OnPropertyChanged(nameof(TreeView));
            }
        }
        public Page DisplayPage
        {
            get => displayPage;
            set
            {
                if (displayPage != value)
                {
                    displayPage = value;
                    OnPropertyChanged(nameof(displayPage));
                }
            }
        }
        public void Navigate(IAlertPage model)
        {
            var page = PageFactory.Generate(model.ValidPageName);
            page.DataContext = model;
            displayPage = page;
        }
    }
}
