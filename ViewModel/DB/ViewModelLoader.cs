using Model;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.DB;

namespace ViewModel.DB
{
    public class LoadContext {
        public AppDBContext DBContext { get; set; }
    }
    static public class ViewModelLoader
    {
        static public ViewModel LoadViewModel()
        {
            var context = new LoadContext() {
                DBContext = new AppDBContext(),
            };
            var viewModel = new DBViewModel(context);
            context.DBContext.SaveChanges();
            return viewModel;
        }
    }
}
