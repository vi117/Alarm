﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alarm.ViewModels
{
    public class AppCommand
    {
        static AppCommand()
        {
            NavigateCommand = new RoutedCommand("Navigate", typeof(AppCommand));
        }
        static public RoutedCommand NavigateCommand{get;}
    }
}