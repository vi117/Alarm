using Alarm.ViewModels.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// ViewModel of All
    /// </summary>
    public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private CollectionViewModel<CategoryViewModel> treeView;
        private Page displayPage;
        public ViewModel()
        {
            Parent = null;
            //test data
            {
                treeView = new CollectionViewModel<CategoryViewModel>(this);
                {
                    var sc = new MockCategoryViewModel("Science");
                    var siteA = new MockFetcherViewModel("A");
                    {
                        var doc = new Document
                        {
                            Title = "News1",
                            Summary = "News1 Summary",
                            Date = DateTime.Now,
                            GUID = "1",
                            Uri = "https://www.naver.com"
                        };
                        siteA.Add(new MockDocumentViewModel(doc));
                        doc = new Document
                        {
                            Title = "News2",
                            Summary = "News2 Summary",
                            Date = DateTime.Now,
                            GUID = "2",
                            Uri = "https://www.google.com"
                        };
                        siteA.Add(new MockDocumentViewModel(doc));
                    }
                    sc.SiteModels.Add(siteA);
                    sc.SiteModels.Add(new MockFetcherViewModel( "B"));
                    sc.SiteModels.Add(new MockFetcherViewModel( "C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new MockCategoryViewModel("Youtube");
                    yt.SiteModels.Add(new MockFetcherViewModel( "A"));
                    yt.SiteModels.Add(new MockFetcherViewModel( "B"));
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
