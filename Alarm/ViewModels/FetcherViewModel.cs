using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public abstract class FetcherViewModel : ViewModelBase, IPageShow
    {
        private bool isSelected;
        private bool isExpanded;

        public string ShowingPageName => "ContentListView";
        public Page ShowingPage { get; set; }

        public abstract CollectionViewModel<DocumentViewModel> Documents { get; set; }

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

        public abstract string Title { get; set; }
        public int FetcherHashId { get; set; }

        public Page CreatePageShowing()
        {
            return new View.ContentListView();
        }
    }
}
