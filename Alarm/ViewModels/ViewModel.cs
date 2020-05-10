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

namespace Alarm.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected IViewModelBehavior root;
        //for designer mode
        public ViewModelBase() { root = null; }
        public ViewModelBase(IViewModelBehavior behavior)
        {
            root = behavior;
        }
        public IViewModelBehavior Root {
            get => root;
            set => root = value;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private CollectionViewModel<CategoryViewModel> treeView;
        private Page displayPage;
        public ViewModel()
        {
            //test data
            {
                treeView = new CollectionViewModel<CategoryViewModel>();
                {
                    var sc = new CategoryViewModel("Science");
                    sc.SiteModels = new CollectionViewModel<SiteViewModel>();
                    var siteA = new SiteViewModel("A");
                    {
                        var doc = new Document
                        {
                            Title = "News1",
                            Summary = "News1 Summary",
                            Date = DateTime.Now,
                            GUID = "1"
                        };
                        siteA.Add(new DocumentViewModel(doc));
                        doc = new Document
                        {
                            Title = "News2",
                            Summary = "News2 Summary",
                            Date = DateTime.Now,
                            GUID = "2"
                        };
                        siteA.Add(new DocumentViewModel(doc));
                    }
                    sc.SiteModels.Add(siteA);
                    sc.SiteModels.Add(new SiteViewModel("B"));
                    sc.SiteModels.Add(new SiteViewModel("C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new CategoryViewModel("Youtube");
                    yt.SiteModels = new CollectionViewModel<SiteViewModel>();
                    yt.SiteModels.Add(new SiteViewModel("A"));
                    yt.SiteModels.Add(new SiteViewModel("B"));
                    treeView.Add(yt);
                }
            }
        }
        public CollectionViewModel<CategoryViewModel> TreeView
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
