using ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public abstract class FetcherViewModel : ViewModelBase, IPageShow
    {
        private bool isSelected;
        private bool isExpanded;

        public string ShowingPageName => "ContentListView";
        public object ShowingPage { get; set; }

        public FetcherViewModel()
        {
            Parent = null;
        }

        public abstract string Title { get; set; }
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
        public abstract ICollectionViewModel<DocumentViewModel> Documents { get; }
    }
}
