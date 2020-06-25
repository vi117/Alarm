using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Alarm.ViewModels
{
    static public class AppCommand
    {
        static AppCommand()
        {
            NavigateCommand = new RoutedCommand("Navigate", typeof(AppCommand));
            ShowSettingWindowCommand = new RoutedCommand("ShowSettingWindow", typeof(AppCommand));
            ShowAddFetcherWindowCommand = new RoutedCommand("ShowAddFetcherWindow", typeof(AppCommand));
            RemoveSelectedCommand = new RoutedCommand("RemoveSelected", typeof(AppCommand));
            ShowEditFetcherWindowCommand = new RoutedCommand("ShowEditFetcherWindow", typeof(AppCommand));
            RefreshFetcherCommand = new RoutedCommand("RefreshFetcher", typeof(AppCommand));
            TranslateCommand = new RoutedCommand("Translate", typeof(AppCommand));
            StartWebBrowserCommand = new RoutedCommand("StartWebBrowser", typeof(AppCommand));
        }
        static public RoutedCommand NavigateCommand { get; }
        static public RoutedCommand ShowSettingWindowCommand { get; }
        static public RoutedCommand ShowAddFetcherWindowCommand { get; }
        static public RoutedCommand RemoveSelectedCommand { get; }
        static public RoutedCommand ShowEditFetcherWindowCommand { get; }
        static public RoutedCommand RefreshFetcherCommand { get; }
        static public RoutedCommand TranslateCommand { get; }
        static public RoutedCommand StartWebBrowserCommand { get; }
    }
}
