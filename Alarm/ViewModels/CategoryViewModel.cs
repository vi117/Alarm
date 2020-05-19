using Alarm.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm.ViewModels
{
    public class CategoryViewModel : ViewModelBase, ICategoryViewModel
    {
        private string title;
        private CollectionViewModel<ISiteViewModel> siteModels;
        private bool isSelected;
        private bool isExpanded;

        /// <summary>
        /// Method for Only Designer Mode.
        /// Do not invoke.
        /// </summary>
        public CategoryViewModel()
        {
            this.title = "No Named Category";
            isSelected = false;
            isExpanded = false;
        }
        public CategoryViewModel(ViewModel viewModel, string title)
        {
            Root = viewModel;
            this.title = title;
            isSelected = false;
            isExpanded = false;
            siteModels = new CollectionViewModel<ISiteViewModel>(viewModel);
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public CollectionViewModel<ISiteViewModel> SiteModels
        {
            get => siteModels;
            set
            {
                siteModels = value;
                OnPropertyChanged(nameof(SiteModels));
            }
        }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    OnPropertyChanged(nameof(isExpanded));
                }
            }
        }

        public string ShowingPageName => "CategoryView";

        public Page ShowingPage { get; set; }

        public Page CreatePageShowing()
        {
            return new View.CategoryView();
        }
    }
}
