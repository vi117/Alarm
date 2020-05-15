using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Alarm.ViewModels
{
    public class CategoryViewModel : ViewModelBase, IPageShow
    {
        private string title;
        private CollectionViewModel<SiteViewModel> siteModels;
        private bool isSelected;
        private bool isExpanded;
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
            siteModels = new CollectionViewModel<SiteViewModel>(viewModel);
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
        public CollectionViewModel<SiteViewModel> SiteModels
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
