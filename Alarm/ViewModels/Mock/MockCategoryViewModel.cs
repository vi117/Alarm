using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm.ViewModels
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
        }
        public MockCategoryViewModel(ViewModel viewModel, string title)
        {
            Root = viewModel;
            this.title = title;
            siteModels = new CollectionViewModel<FetcherViewModel>(viewModel);
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
        public override CollectionViewModel<FetcherViewModel> SiteModels
        {
            get => siteModels;
            set
            {
                siteModels = value;
                OnPropertyChanged(nameof(SiteModels));
            }
        }
    }
}
