using Alarm.ViewModels.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Diagnostics;
using System.Windows.Input;

namespace Alarm.ViewModels
{
    /// <summary>
    /// ViewModel of All
    /// </summary>
    abstract public class ViewModel : ViewModelBase, IViewModelBehavior
    {
        private Page displayPage;
        public ViewModel()
        {
            Parent = null;
        }
        abstract public CollectionViewModel<CategoryViewModel> TreeView { get; set; }

        public Page DisplayPage
        {
            get => displayPage;
            set
            {
                if (displayPage != value)
                {
                    displayPage = value;
                    OnPropertyChanged(nameof(displayPage));
                }
            }
        }
        //Navigate page
        public void Navigate(IPageShow model)
        {
            var page = PageFactory.Generate(model);
            DisplayPage = page;
        }
    }
}
