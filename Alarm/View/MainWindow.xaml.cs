using Alarm.ViewModels;
using Model;
using Model.DB;
using System;
using System.Diagnostics;
using System.Windows.Input;
using ViewModel;
using ViewModel.DB;
using ViewModel.Updater;
using MessageBox = System.Windows.MessageBox;

namespace Alarm.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        static private ViewModel.ViewModel viewModel;
        static private DocumentPublisher publisher;

        public ViewModel.ViewModel WindowViewModel
        {
            get => viewModel;
        }
        void BindCommand()
        {
            CommandBindings.Add(new CommandBinding(AppCommand.NavigateCommand,
                (sender, eventArgs) => {
                    if(eventArgs.Parameter != null)
                        viewModel.Navigate(eventArgs.Parameter as IPageShow, PageFactory.Factory);
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
                        fetcher.Interval = TimeSpan.FromSeconds(30);
                        var item = NavTreeView.SelectedItem;
                        if (item == null)
                        {
                            MessageBox.Show("Select Category First");
                        }
                        else if (item is DBCategoryViewModel c)
                        {
                            var fetcherView = window.GetFetcherViewModel();
                            c.SitesModelDetail.Emplace(fetcherView.Title, fetcher);
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
            viewModel = ViewModel.DB.ViewModelLoader.LoadViewModel(publisher);
            //viewModel.TreeView.Add(new DBCategoryViewModel());
            DataContext = viewModel;
            BindCommand();
        }
    }
}
