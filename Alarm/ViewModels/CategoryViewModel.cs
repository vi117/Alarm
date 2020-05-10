using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarm.ViewModels
{
    public class CategoryViewModel : ViewModelBase, IAlertPage
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
        public CategoryViewModel(string title)
        {
            this.title = title;
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

        public string ValidPageName => "CategoryView";
    }
}
