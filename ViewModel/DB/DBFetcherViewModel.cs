using ViewModel.Interface;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DB
{
    class DBFetcherViewModel : FetcherViewModel
    {
        int fetcherId;

        public DBFetcherViewModel(int fetcherId)
        {
            this.fetcherId = fetcherId;
        }

        public override ICollectionViewModel<DocumentViewModel> Documents { 
            get => throw new NotImplementedException(); 
        }
        public override string Title { 
            get {
                using (var context = new AppDBContext())
                {
                    var fetcher = context.Fetchers.Find(fetcherId);
                    return fetcher.Title;
                }
            
            } set => throw new NotImplementedException(); }
    }
}
