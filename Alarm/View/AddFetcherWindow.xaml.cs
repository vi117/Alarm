using Alarm.View.FetcherForm;
using Publisher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace Alarm.View
{
    /// <summary>
    /// AddFetcherWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddFetcherWindow : MahApps.Metro.Controls.MetroWindow
    {
        FetcherForm.FetcherFormControl fetcherView;
        Dictionary<string, FetcherFormControl> keyValuePairs;
        public AddFetcherWindow()
        {
            InitializeComponent();
            keyValuePairs = new Dictionary<string, FetcherFormControl>() {
                ["RSS"] = new RSSForm(),
                ["Atom"] = new AtomForm()
            };
            fetcherView = null;
        }

        public Fetcher GetFetcher()
        {
            if (fetcherView == null)
                return null;
            else
                return fetcherView.GetFetcher();
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
        private void SetUserControl(FetcherFormControl elem)
        {
            var showingAnimation = FindResource("ShowingElement") as DoubleAnimation;
            var disposeAnimation = FindResource("DisposeElement") as DoubleAnimation;
            var uIElement = fetcherView as UIElement;
            EventHandler disposeAnimationCompleted = (s, e) => { };
            disposeAnimationCompleted = (s, e) =>
            {
                fetcherView = elem;
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
            var item = comboBox.SelectedItem as ComboBoxItem;
            var query = item.Content.ToString();
            SetUserControl(keyValuePairs[query]);
        }
    }
}
