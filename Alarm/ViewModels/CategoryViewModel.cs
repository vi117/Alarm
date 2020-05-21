﻿using Alarm.ViewModels.Interface;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public abstract class CategoryViewModel : ViewModelBase, IPageShow
    {
        public string ShowingPageName => "CategoryView";
        public Page ShowingPage { get; set; }
        private bool isSelected = false;
        private bool isExpanded = false;


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
        public abstract CollectionViewModel<FetcherViewModel> SiteModels { get; set; }
        public abstract string Title { get; set; }

        public Page CreatePageShowing()
        {
            return new View.CategoryView();
        }
    }
}