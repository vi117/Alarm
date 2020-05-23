using ViewModel.Interface;
using Model;
using Model.DB;
using System;

namespace ViewModel
{
    public class MockViewModel : ViewModel
    {
        private CollectionViewModel<CategoryViewModel> treeView;
        public MockViewModel()
        {
            var context = new AppDBContext();
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
                    sc.SiteModels.Add(new MockFetcherViewModel("B"));
                    sc.SiteModels.Add(new MockFetcherViewModel("C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new MockCategoryViewModel("Youtube");
                    yt.SiteModels.Add(new MockFetcherViewModel("A"));
                    yt.SiteModels.Add(new MockFetcherViewModel("B"));
                    treeView.Add(yt);
                }
            }
        }
        public CollectionViewModel<CategoryViewModel> DesignerTreeView
        {
            get => treeView;
        }
        public override ICollectionViewModel<CategoryViewModel> TreeView
        {
            get => treeView;
        }
    }
}
