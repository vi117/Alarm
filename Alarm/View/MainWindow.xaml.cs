using Alarm.ViewModels;
using Alarm.ViewModels.Updater;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace Alarm.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        static private ViewModel viewModel;
        static private DocumentPublisher publisher;

        public ViewModel WindowViewModel
        {
            get => viewModel;
        }
        void BindCommand()
        {
            CommandBindings.Add(new CommandBinding(AppCommand.NavigateCommand,
                (sender, eventArgs) => {
                    viewModel.Navigate(eventArgs.Parameter as IPageShow);
                    eventArgs.Handled = true;
                },
                (s, e) => { e.CanExecute = true; }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.ShowSettingWindowCommand,
                (sender, eventArgs) => {
                    SettingWindow window = new SettingWindow();
                    window.ShowDialog();
                    eventArgs.Handled = true;
                },
                (s, e) => { e.CanExecute = true; }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.ShowAddFetcherWindowCommand,
                (sender, eventArgs) => {
                AddFetcherWindow window = new AddFetcherWindow();
                bool? b = window.ShowDialog();
                if (b.HasValue && b.Value)
                {
                    var fetcher = window.GetFetcher();
                    fetcher.Interval = TimeSpan.FromSeconds(1);
                    publisher.AddFetcher(fetcher);
                    var item = NavTreeView.SelectedItem;
                    if (item == null)
                    {
                        MessageBox.Show("Select Category First");
                    }
                    else if (item.GetType().BaseType == typeof(CategoryViewModel)) {
                        var c = item as CategoryViewModel;
                            var fetcherView = window.GetFetcherViewModel();
                            c.SiteModels.Add(fetcherView);
                            var updater = new MockFetcherViewModelUpdater(fetcher,fetcherView);
                            publisher.RegisterUpdater(updater);
                        }
                        else
                        {
                            Trace.WriteLine("Not impletemet");
                            throw new NotImplementedException();
                        }
                    }
                    eventArgs.Handled = true;
                },
                (s, e) => { e.CanExecute = true; }
            ));
        }
        public MainWindow()
        {
            InitializeComponent();
            publisher = new DocumentPublisher();
            viewModel = new MockViewModel();
            DataContext = viewModel;
            BindCommand();
        }
    }
}
