using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    class DBCategoryViewModel : CategoryViewModel
    {
        DBCategory category;
        public DBCategoryViewModel(DBCategory category)
        {
            this.category = category;
        }
        public override string Title { 
            get => category.Title; 
            set => category.Title = value;
        }
        public DBCategory DBCategory { get => category; set => category = value; }

        public override
            ICollectionViewModel<FetcherViewModel> 
            SiteModels {
            get => throw new NotImplementedException();
        }
    }
}
