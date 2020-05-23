using ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel
{
    public class MockCategoryViewModel : CategoryViewModel
    {
        private string title;
        private CollectionViewModel<FetcherViewModel> siteModels;

        /// <summary>
        /// Method for Only Designer Mode.
        /// Do not invoke.
        /// </summary>
        public MockCategoryViewModel()
        {
            this.title = "No Named Category";
            siteModels = new CollectionViewModel<FetcherViewModel>(this);
        }
        public MockCategoryViewModel(string title)
        {
            this.title = title;
            siteModels = new CollectionViewModel<FetcherViewModel>(this);
        }
        public override string Title
        {
            get {
                Trace.Write(title); 
                return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public CollectionViewModel<FetcherViewModel> DesignerSiteModels
        {
            get => siteModels;
        }
        public override
            ICollectionViewModel<FetcherViewModel> 
            SiteModels
        {
            get => siteModels;
        }
    }
}
