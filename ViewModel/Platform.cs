using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace ViewModel
{
    public interface Platform
    {
        void EnableCollectionSynchronization(IEnumerable collection, object lockObject);
        /// <summary>
        /// Fire the <c>CollectionChangedEvent</c> for multitask environment.
        /// Must use it when fire the event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventHandler"></param>
        /// <param name="e"></param>
        void CollectionChangedInvoke(object sender, NotifyCollectionChangedEventHandler eventHandler, NotifyCollectionChangedEventArgs e);
        /// <summary>
        /// Fire the <c>PropertyChangedEvent</c> for multitask environment.
        /// Must use it when fire the event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventHandler"></param>
        /// <param name="e"></param>
        void PropertyChangedInvoke(object sender, PropertyChangedEventHandler eventHandler, PropertyChangedEventArgs e);
    }
    public static class PlatformSevice { 
        private static Platform instance = new DefaultPlatformService();
        public static Platform Instance { 
            get {
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

        public void PropertyChangedInvoke(object sender, PropertyChangedEventHandler eventHandler, PropertyChangedEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
    }
}
