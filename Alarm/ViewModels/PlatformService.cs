using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using ViewModel;

namespace Alarm.ViewModels
{
    public class WPFPlatform : ViewModel.Platform
    {
        public void CollectionChangedInvoke(object sender, NotifyCollectionChangedEventHandler eventHandler, NotifyCollectionChangedEventArgs e)
        {
            if (eventHandler == null) return;
            foreach (NotifyCollectionChangedEventHandler nh in eventHandler.GetInvocationList())
            {
                if (nh.Target is DispatcherObject dispObj)
                {
                    Dispatcher dispatcher = dispObj.Dispatcher; //그 쓰레드의 디스패처에 접근해서
                    if (dispatcher != null && !dispatcher.CheckAccess()) // 액세스 불가능하면
                    {
                        dispatcher.BeginInvoke(//STA thread의 디스패처에게 가서 이벤트를 보냄.
                            (Action)(() => nh.Invoke(sender, e)),// new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                            DispatcherPriority.DataBind);
                        continue;
                    }
                }
                nh.Invoke(sender, e);
            }
        }

        public void EnableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            BindingOperations.EnableCollectionSynchronization(collection, lockObject);
        }
        public static void Register()
        {
            PlatformSevice.Instance = new WPFPlatform();
        }
    }
}
