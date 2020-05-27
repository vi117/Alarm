using Alarm.ViewModels;
using ControlzEx.Standard;
using Model;
using Model.DB;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModel;
using ViewModel.DB;
using ViewModel.Updater;
using Alarm.Helper;
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
        void AddFetcherWindow(CategoryViewModel categoryView)
        {
            AddFetcherWindow window = new AddFetcherWindow();
            bool? b = window.ShowDialog();
            if (b.HasValue && b.Value)
            {
                var fetcher = window.GetFetcher();
                categoryView.Emplace(window.GetTitle(), fetcher);
            }
        }
        void AddCategoryView()
        {
            var window = new CategoryDialog();
            bool? b = window.ShowDialog();
            if (b == true)
            {
                viewModel.EmplaceCategory(window.getTitleName());
            }
        }
        bool IsDialogShowing(Type type)
        {
            foreach(var window in App.Current.Windows)
            {
                if (window.GetType().IsAssignableFrom(type))
                    return true;
            }
            return false;
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
                (s, e) => { e.CanExecute = !IsDialogShowing(typeof(SettingWindow)); }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.ShowAddFetcherWindowCommand,
                (sender, eventArgs) => {
                    switch (NavTreeView.SelectedItem) {
                        case FetcherViewModel s:
                            AddFetcherWindow(s.Parent as CategoryViewModel);
                            break;
                        case CategoryViewModel c when c.SiteModels.Count() == 0:
                            AddFetcherWindow(c);
                            break;
                        default:
                            AddCategoryView();
                            break;
                    }
                    eventArgs.Handled = true;
                },
                (s, e) => {
                    e.CanExecute = !IsDialogShowing(typeof(AddFetcherWindow))
                && !IsDialogShowing(typeof(CategoryDialog));
                }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.RemoveSelectedCommand,
                (sender, eventArgs) =>
                {
                    switch (NavTreeView.SelectedItem)
                    {
                        case CategoryViewModel category:
                            (category.Parent as ViewModel.ViewModel).RemoveCategory(category);
                            break;
                        case FetcherViewModel fetcher:
                            (fetcher.Parent as CategoryViewModel).Remove(fetcher);
                            break;
                        case null:
                            break;
                        default:
                            throw new InvalidOperationException("Unreachable!");
                    }
                    eventArgs.Handled = true;
                }, (s, e) => e.CanExecute = true
                ));
        }
        public MainWindow()
        {
            InitializeComponent();
            publisher = new DocumentPublisher();
            viewModel = ViewModel.DB.ViewModelLoader.LoadViewModel(publisher);
            DataContext = viewModel;
            BindCommand();
        }



        Point startPoint;
        private void NavTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }
        private void NavTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(null);
                var diff = startPoint - mousePos;
                if(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    ||Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var treeView = sender as TreeView;
                    var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem
                        (treeView.SelectedItem) as TreeViewItem;
                    //category 항목은 drag & drop을 적용안하기에 나감.
                    if (treeViewItem != null) return;
                    //category 밑 fetcher 항목에서 찾는다.
                    foreach (var subItem in treeView.Items)
                    {
                        treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(subItem)
                            as TreeViewItem;
                        treeViewItem = treeViewItem?.ItemContainerGenerator.ContainerFromItem(treeView.SelectedItem)
                            as TreeViewItem;
                        if (treeViewItem != null) break;
                    }
                    if (treeViewItem == null) return;
                    var fetcherViewModel = treeView.SelectedItem as FetcherViewModel;
                    if (fetcherViewModel == null) return;
                    var dragData = new DataObject("FetcherViewModel",fetcherViewModel);
                    //treeViewItem 게 밑바닥으로 깔려야하는 데 왜 안될까?
                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                }
            }
        }
        private void NavTreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(FetcherViewModel)))
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void NavTreeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FetcherViewModel"))
            {
                var fetcherViewModel = e.Data.GetData("FetcherViewModel") as FetcherViewModel;
                var treeView = sender as TreeView;
                CategoryViewModel category = null;
                foreach (var subItem in treeView.Items)
                {
                    category = subItem as CategoryViewModel;
                    var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(subItem) as TreeViewItem;
                    if (treeViewItem.RenderSize.IsInRelative(e.GetPosition(treeViewItem)))
                    {
                        break;
                    }
                }
                if (category == null) throw new ApplicationException("Search fail.");
                if(fetcherViewModel.Parent != category)
                {
                    fetcherViewModel.ChangeOwner(category);
                }
                e.Handled = true;
            }
        }

    }
}
