using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interface
{
    interface INotifyPublished
    {
        event PublishedEventHandler OnPublished;
    }
}
