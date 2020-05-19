using Model;
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
        private IViewModelBehavior root;
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
    /// <summary>
    /// ViewModel of All
    /// </summary>
    public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private CollectionViewModel<CategoryViewModel> treeView;
        private Page displayPage;
        public ViewModel()
        {
            Root = this;
            //test data
            {
                treeView = new CollectionViewModel<CategoryViewModel>(Root);
                {
                    var sc = new CategoryViewModel(this,"Science");
                    var siteA = new SiteViewModel(this,"A");
                    {
                        var doc = new Document
                        {
                            Title = "News1",
                            Summary = "News1 Summary",
                            Date = DateTime.Now,
                            GUID = "1",
                            Uri = "https://www.naver.com"
                        };
                        siteA.Add(new DocumentViewModel(doc));
                        doc = new Document
                        {
                            Title = "News2",
                            Summary = "News2 Summary",
                            Date = DateTime.Now,
                            GUID = "2",
                            Uri = "https://www.google.com"
                        };
                        siteA.Add(new DocumentViewModel(doc));
                    }
                    sc.SiteModels.Add(siteA);
                    sc.SiteModels.Add(new SiteViewModel(this, "B"));
                    sc.SiteModels.Add(new SiteViewModel(this, "C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new CategoryViewModel(this,"Youtube");
                    yt.SiteModels.Add(new SiteViewModel(this, "A"));
                    yt.SiteModels.Add(new SiteViewModel(this, "B"));
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
        //Navigate page
        public void Navigate(IPageShow model)
        {
            var page = PageFactory.Generate(model);
            DisplayPage = page;
        }
    }
}
