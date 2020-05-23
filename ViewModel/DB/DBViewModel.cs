using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    public class DBViewModel : ViewModel
    {
        private CollectionViewModel<CategoryViewModel> categories;

        public DBViewModel()
        {
            var context = new AppDBContext();
            var DBCategories = context.Categorys.ToList()
                .Select((x)=>(CategoryViewModel)new DBCategoryViewModel(x))
                .OrderBy((x)=>x.Title)
                .ToList();
            categories = new CollectionViewModel<CategoryViewModel>(DBCategories);
        }

        public override ICollectionViewModel<CategoryViewModel> TreeView { 
            get {
                return categories;
            }
        }
    }
}
