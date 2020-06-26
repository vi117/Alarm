using ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using Model;

namespace ViewModel
{
    public class MockCategoryViewModel : CategoryViewModel
    {
        private string title;
        
        private MockCollectionViewModel<FetcherViewModel> siteModels;

        //xaml 의 이해할 수 없는 오류로 인해 조치함.
        public class DesignerHelper : ObservableCollection<MockFetcherViewModel>
        {
            MockCollectionViewModel<FetcherViewModel> target;
            public DesignerHelper(MockCollectionViewModel<FetcherViewModel> t)
            {
                target = t;
            }
            protected override void InsertItem(int index, MockFetcherViewModel item)
            {
                base.InsertItem(index, item);
                target.Insert(index, item);
            }
        }

        /// <summary>
        /// Method for Only Designer Mode.
        /// Do not invoke.
        /// </summary>
        public MockCategoryViewModel()
        {
            this.title = "No Named Category";
            siteModels = new MockCollectionViewModel<FetcherViewModel>(this);
        }
        public MockCategoryViewModel(string title)
        {
            this.title = title;
            siteModels = new MockCollectionViewModel<FetcherViewModel>(this);
        }
        public override string Title
        {
            get {
                return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public DesignerHelper DesignerSiteModels
        {
            get => new DesignerHelper(siteModels);
        }
        public MockCollectionViewModel<FetcherViewModel> SiteModelsDetail
        {
            get => siteModels;
        }
        public override
            ICollectionViewModel<FetcherViewModel> 
            SiteModels
        {
            get => siteModels;
        }

        public override void Emplace(string title,Fetcher fetcher)
        {
            siteModels.Add(new MockFetcherViewModel() { Title = title });
        }

        public override bool Remove(FetcherViewModel fetcherViewModel)
        {
            fetcherViewModel.Fetcher.Stop();
            return siteModels.Remove(fetcherViewModel);
        }
    }
}
