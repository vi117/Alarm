using Alarm.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ViewModel;

namespace Alarm.View.FetcherForm
{
    ///<summary>
    ///Wrapper class for <c>UserControl</c> and <c>IFetcherView</c>
    ///</summary>
    abstract public class FetcherFormControl : UserControl, IFetcherView
    {
        abstract public Fetcher CreateFetcher();
        /// <exception cref="InvalidCastException"></exception>
        /// <param name="fetcher"></param>
        abstract public void SetFromFetcher(Fetcher fetcher);
        abstract public string FetcherName
        {
            get; set;
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class FetcherFormAttribute : Attribute
    {
        readonly Type targetType;
        readonly string name;

        public FetcherFormAttribute(string name,Type targetType)
        {
            this.targetType = targetType;
            this.name = name;
        }

        public Type FatcherType
        {
            get => targetType;
        }
        public string Name {
            get => name;
        }
    }
    internal static class FetcherFormAttributeHelper {
        static Dictionary<string,ConstructorInfo> NameConstructorPairs;
        internal static Dictionary<Type, MethodInfo> FetcherTypeMethodPairs;
        internal static Dictionary<Type, string> FatcherTypeToNamePairs;

        static FetcherFormAttributeHelper(){
            var allTypes = typeof(FetcherFormControl).Assembly.GetTypes();
            var customTypes = from t in allTypes 
                              where t.GetCustomAttribute<FetcherFormAttribute>() != null 
                              select t;
            var attrs = from c in customTypes 
                        select c.GetCustomAttribute<FetcherFormAttribute>();
            var zipped = customTypes.Zip(attrs, (x, y) => (x, y));
            FatcherTypeToNamePairs = attrs.ToDictionary(x => x.FatcherType, x => x.Name);
            FetcherTypeMethodPairs = zipped.ToDictionary(x => x.y.FatcherType, x => x.x.GetMethod("SetFromFetcher"));
            NameConstructorPairs = zipped.ToDictionary(x => x.y.Name , x => x.x.GetConstructor(new Type[] { }));
        }
        static internal Dictionary<string,FetcherFormControl> GetForms()
        {
            return NameConstructorPairs.ToDictionary(
                x => x.Key, 
                x => (FetcherFormControl)x.Value.Invoke(new object[] { })
                );
        }
    }
}
