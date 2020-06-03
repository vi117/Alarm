using ViewModel.Interface;
using Model;
using Model.DB;
using System;

namespace ViewModel
{
    public class MockViewModel : ViewModel
    {
        private MockCollectionViewModel<CategoryViewModel> treeView;
        public MockViewModel()
        {
            {
                treeView = new MockCollectionViewModel<CategoryViewModel>(this);
                {
                    var sc = new MockCategoryViewModel("Science");
                    var siteA = new MockFetcherViewModel("A");
                    {
                        var doc = new PubDocument
                        {
                            Title = "News1",
                            Summary = "News1 Summary",
                            Date = DateTime.Now,
                            GUID = "1",
                            Uri = "https://www.naver.com"
                        };
                        siteA.Add(new MockDocumentViewModel(doc));
                        doc = new PubDocument
                        {
                            Title = "News2",
                            Summary = "News2 Summary",
                            Date = DateTime.Now,
                            GUID = "2",
                            Uri = "https://www.google.com"
                        };
                        siteA.Add(new MockDocumentViewModel(doc));
                    }
                    sc.SiteModelsDetail.Add(siteA);
                    sc.SiteModelsDetail.Add(new MockFetcherViewModel("B"));
                    sc.SiteModelsDetail.Add(new MockFetcherViewModel("C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new MockCategoryViewModel("Youtube");
                    yt.SiteModelsDetail.Add(new MockFetcherViewModel("A"));
                    yt.SiteModelsDetail.Add(new MockFetcherViewModel("B"));
                    treeView.Add(yt);
                }
            }
        }
        public MockCollectionViewModel<CategoryViewModel> DesignerTreeView
        {
            get => treeView;
        }
        public override ICollectionViewModel<CategoryViewModel> TreeView
        {
            get => treeView;
        }

        public override void EmplaceCategory(string title)
        {
            treeView.Add(new MockCategoryViewModel(title));
        }

        public override bool RemoveCategory(CategoryViewModel categoryViewModel)
        {
            return treeView.Remove(categoryViewModel);
        }
    }
}
