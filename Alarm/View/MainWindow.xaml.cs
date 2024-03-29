﻿using Alarm.ViewModels;
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
using Alarm.Helper;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Interactivity;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro;
using System.Windows.Media.Animation;
using Alarm.Language;

namespace Alarm.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        static private ViewModel.ViewModel viewModel;

        public ViewModel.ViewModel WindowViewModel
        {
            get => viewModel;
        }
        void AddFetcherWindow(CategoryViewModel categoryView)
        {
            AddFetcherWindow window = new AddFetcherWindow();
            window.Owner = this;
            bool? b = window.ShowDialog();
            if (b.HasValue && b.Value)
            {
                var fetcher = window.GetFetcher();
                categoryView.Emplace(window.GetFetcherTitle(), fetcher);
            }
        }
        async void AddCategoryView()
        {
            var t = await this.ShowInputAsync(
                this.GetText("AddCategoryTitle"),
                this.GetText("AddCategoryMessage"));
            if (t != null && t != string.Empty)
            {
                viewModel.EmplaceCategory(t);
            }
        }
        async void RemoveCategoryView(CategoryViewModel category)
        {
            var t = await this.ShowMessageAsync(
                this.GetText("RemoveCategoryTitle"),
                this.GetText("RemoveCategoryMessage"),
                MessageDialogStyle.AffirmativeAndNegative);
            if(t == MessageDialogResult.Affirmative)
                (category.Parent as ViewModel.ViewModel).RemoveCategory(category);
        }
        void EditFetcherView(FetcherViewModel fetcher)
        {
            AddFetcherWindow window = new AddFetcherWindow(fetcher.Title,fetcher.Fetcher);
            window.Owner = this;
            bool? b = window.ShowDialog();
            if (b.HasValue && b.Value)
            {
                fetcher.Title = window.GetFetcherTitle();
                fetcher.Fetcher = window.GetFetcher();
            }
        }
        async void EditCategoryView(CategoryViewModel category)
        {
            var t = await this.ShowInputAsync(
                this.GetText("EditCategoryTitle"),
                this.GetText("EditCategoryMessage"));
            if (t != null && t != string.Empty)
            {
                category.Title = t;
            }
        }
        bool IsDialogShowing(Type type)
        {
            foreach (var window in App.Current.Windows)
            {
                if (window.GetType().IsAssignableFrom(type))
                    return true;
            }
            return false;
        }
        void BindCommand()
        {
            CommandBindings.Add(new CommandBinding(AppCommand.NavigateCommand,
                (sender, eventArgs) =>
                {
                    if (eventArgs.Parameter != null)
                        viewModel.Navigate(eventArgs.Parameter as IPageShow, PageFactory.Factory);
                    eventArgs.Handled = true;
                },
                (s, e) => { e.CanExecute = true; }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.ShowSettingWindowCommand,
                (sender, eventArgs) =>
                {
                    SettingWindow window = new SettingWindow();
                    window.Owner = this;
                    window.ShowDialog();
                    eventArgs.Handled = true;
                },
                (s, e) => { e.CanExecute = !IsDialogShowing(typeof(SettingWindow)); }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.ShowAddFetcherWindowCommand,
                (sender, eventArgs) =>
                {
                    switch (NavTreeView.SelectedItem)
                    {
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
                (s, e) =>
                {
                    e.CanExecute = !IsDialogShowing(typeof(AddFetcherWindow));
                }
            ));
            CommandBindings.Add(new CommandBinding(AppCommand.RemoveSelectedCommand,
                (sender, eventArgs) =>
                {
                    switch (NavTreeView.SelectedItem)
                    {
                        case CategoryViewModel category:
                                RemoveCategoryView(category);
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
            CommandBindings.Add(new CommandBinding(AppCommand.ShowEditFetcherWindowCommand,
                (sender, eventArgs) =>
                {
                    switch (NavTreeView.SelectedItem)
                    {
                        case CategoryViewModel category:
                            EditCategoryView(category);
                            break;
                        case FetcherViewModel fetcher:
                            EditFetcherView(fetcher);
                            break;
                        case null:
                            break;
                        default:
                            throw new InvalidOperationException("Unreachable!");
                    }
                    eventArgs.Handled = true;
                }, (s, e) => e.CanExecute = true
                ));
            CommandBindings.Add(new CommandBinding(AppCommand.RefreshFetcherCommand,
                (sender, eventArgs) => {
                    var displayed = (Page)viewModel.DisplayPage;
                    switch (displayed?.DataContext)
                    {
                        case FetcherViewModel fetcher:
                            fetcher.Refresh();
                            break;
                        case CategoryViewModel category:
                            category.RefreshAll();
                            break;
                        case DocumentViewModel _:
                            ((ContentView)displayed).ReloadBrowser();
                            break;
                        case null:
                            break;
                        default:
                            throw new InvalidOperationException("Unreachable!");
                    }
                }));
            CommandBindings.Add(new CommandBinding(AppCommand.TranslateCommand, 
                (sender,eventArgs) => {
                    var displayed = (Page)viewModel.DisplayPage;
                    switch (displayed?.DataContext)
                    {
                        case FetcherViewModel _:
                            ((ContentListView)displayed).TranslateSelected();
                            break;
                        case CategoryViewModel _:
                        case DocumentViewModel _:
                        case null:
                            this.ShowMessageAsync(
                                this.GetText("AlertTitle"),
                                this.GetText("TranslateCommandSelectWrongObject"));
                            break;
                        default:
                            throw new InvalidOperationException("Unreachable!");
                    }
                }));
            CommandBindings.Add(new CommandBinding(AppCommand.StartWebBrowserCommand,
                (sender, eventArgs) => {
                    var uri = eventArgs.Parameter as string;
                    if(uri != null)
                        Process.Start(uri);
                }));
        }
        
        public MainWindow()
        {
            InitializeComponent();
            viewModel = ViewModelLoader.LoadViewModel();
            DataContext = viewModel;
            RegisterTrayIcon();
            BindCommand();
        }

        private System.Windows.Forms.NotifyIcon notifyIcon;
        public void RegisterTrayIcon()
        {
            var contextMenu = new System.Windows.Forms.ContextMenu();
            var ItemShow = new System.Windows.Forms.MenuItem()
            {
                Index = 0,
                Text = "Show"
            };
            ItemShow.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                }
            };
            var ItemExit = new System.Windows.Forms.MenuItem()
            {
                Index = 1,
                Text="Exit"
            };
            contextMenu.MenuItems.Add(ItemShow);
            contextMenu.MenuItems.Add(ItemExit);
            ItemExit.Click += (s, e) => { this.Close(); };

            notifyIcon = new System.Windows.Forms.NotifyIcon() {
                Icon = Properties.Resources.speed,
                Visible = true,
                ContextMenu = contextMenu,
                Text = Properties.Settings.Default.Title
            };

            notifyIcon.DoubleClick +=(sender, args) => 
                {
                    Show();
                    WindowState = WindowState.Normal;
                };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            notifyIcon.Visible = false;
            base.OnClosed(e);
        }
        Point startPoint;
        private void NavTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void NavTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(null);
                var diff = startPoint - mousePos;
                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
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
                    var dependencyObject = (DependencyObject)e.OriginalSource;
                    var col = dependencyObject.GetSelfAndAncestors().Where((x) => x == treeViewItem).Count();
                    if (col == 0) return;

                    var fetcherViewModel = treeView.SelectedItem as FetcherViewModel;
                    if (fetcherViewModel == null) return;
                    var dragData = new DataObject("FetcherViewModel", fetcherViewModel);
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
                if (fetcherViewModel.Parent != category)
                {
                    fetcherViewModel.ChangeOwner(category);
                }
            }
            e.Handled = true;
        }

    }
}
