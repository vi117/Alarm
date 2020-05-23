using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ViewModel
{
    public interface Platform
    {
        void EnableCollectionSynchronization(IEnumerable collection, object lockObject);
        void CollectionChangedInvoke(object sender, NotifyCollectionChangedEventHandler eventHandler, NotifyCollectionChangedEventArgs e);
    }
    public static class PlatformSevice { 
        private static Platform instance = null;
        public static Platform Instance { 
            get {
                if (instance == null) throw new InvalidOperationException("platform not ready");
                return instance;
            }
            set => instance = value; 
        }
    }
    public class DefaultPlatformService : Platform
    {
        public void CollectionChangedInvoke(object sender,NotifyCollectionChangedEventHandler eventHandler, NotifyCollectionChangedEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        public void EnableCollectionSynchronization(IEnumerable collection, object lockObject){}
    }
}
