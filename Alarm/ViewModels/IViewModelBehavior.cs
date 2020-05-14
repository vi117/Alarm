﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Alarm.ViewModels
{
    public interface IViewModelBehavior
    {
        void Navigate(IPageShow page);
    }
}