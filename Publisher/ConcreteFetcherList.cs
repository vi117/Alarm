using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Model
{
    static public class ConcreteFetcherList {
        public static readonly Type[] types;
        static ConcreteFetcherList(){
            types = typeof(Fetcher).Assembly.GetTypes()
                .Where((x) => x.IsSubclassOf(typeof(Fetcher)))
                .ToArray();
        }
    }
}
