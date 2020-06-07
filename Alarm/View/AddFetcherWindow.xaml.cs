using Alarm.View.FetcherForm;
using Alarm.ViewModels;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;

namespace Alarm.View
{
    /// <summary>
    /// AddFetcherWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddFetcherWindow : MahApps.Metro.Controls.MetroWindow
    {
        FetcherForm.FetcherFormControl fetcherView;
        Dictionary<string, FetcherFormControl> keyValuePairs;
        public AddFetcherWindow(string title,Fetcher fetcher):this()
        {
            Title = "Edit Fetcher Window";
            AddButton.Content = "Edit";
            MethodInfo methodInfo;
            var fetcherType = fetcher.GetType();
            bool b = FetcherFormAttributeHelper.FetcherTypeMethodPairs.TryGetValue(fetcherType, out methodInfo);
            if (!b) throw new ArgumentException("The argument doesn't implemenet " + nameof(FetcherFormAttribute));
            var name = FetcherFormAttributeHelper.FatcherTypeToNamePairs[fetcherType];
            methodInfo.Invoke(keyValuePairs[name], new object[] { fetcher });
            keyValuePairs[name].FetcherName = title;
            ContentTypeComboBox.SelectedIndex = keyValuePairs.Keys.OrderBy(x => x).ToList().IndexOf(name);
        }
        public AddFetcherWindow()
        {
            InitializeComponent();
            keyValuePairs = FetcherFormAttributeHelper.GetForms();
            fetcherView = null;
            ContentTypeComboBox.ItemsSource = keyValuePairs.Keys.OrderBy(x=>x).ToList();
        }

        public Fetcher GetFetcher()
        {
            if (fetcherView == null)
                return null;
            else
                return fetcherView.CreateFetcher();
        }
        public string GetFetcherTitle()
        {
            return fetcherView?.FetcherName;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = (fetcherView != null);
            e.Handled = true;
        }
        /// <summary>
        /// Set the View
        /// </summary>
        /// <param name="elem">UIElement to replace</param>
        private void SetUserControl(UIElement elem)
        {
            var showingAnimation = FindResource("ShowingElement") as DoubleAnimation;
            var disposeAnimation = FindResource("DisposeElement") as DoubleAnimation;
            var uIElement = fetcherView as UIElement;
            EventHandler disposeAnimationCompleted = (s, e) => { };
            disposeAnimationCompleted = (s, e) =>
            {
                fetcherView = (FetcherFormControl)elem;
                FetcherContentParent.Children.Clear();
                FetcherContentParent.Children.Add(elem);
                disposeAnimation.Completed -= disposeAnimationCompleted;
                elem.BeginAnimation(OpacityProperty, showingAnimation);
            };
            disposeAnimation.Completed += disposeAnimationCompleted;
            if (uIElement == null)
                disposeAnimationCompleted.Invoke(this, new EventArgs());
            else
                uIElement.BeginAnimation(OpacityProperty, disposeAnimation);
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var item = comboBox.SelectedItem as string;
            SetUserControl(keyValuePairs[item]);
        }
    }
}
