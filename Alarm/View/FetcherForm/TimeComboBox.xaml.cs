using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm.View.FetcherForm
{
    /// <summary>
    /// TimeComboBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimeComboBox : UserControl
    {
        static Dictionary<string, TimeSpan> keyValuePairs = new Dictionary<string, TimeSpan>()
        {
            ["1 minute"] = TimeSpan.FromMinutes(1),
            ["5 minutes"] = TimeSpan.FromMinutes(5),
            ["30 minutes"] = TimeSpan.FromMinutes(30),
            ["1 hour"] = TimeSpan.FromHours(1),
            ["3 hours"] = TimeSpan.FromHours(3),
            [CustomContnet] = TimeSpan.FromDays(1)
        };
        static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime",
                typeof(TimeSpan), typeof(TimeComboBox),
                new PropertyMetadata(TimeSpan.FromMinutes(30)));

        const string CustomContnet = "Custom";

        public string DefaultText { get => "Select the time"; }

        public TimeComboBox()
        {
            InitializeComponent();
            IntervalBox.ItemsSource = keyValuePairs.Keys;
        }

        public TimeSpan SelectedTime
        {
            get => (TimeSpan)GetValue(SelectedTimeProperty);
            set {
                IntervalBox.SelectedItem = keyValuePairs.FirstOrDefault(x => x.Value == value).Key;
                SetValue(SelectedTimeProperty, value);
            }
        }

        async private void IntervalBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var key = (string)IntervalBox.SelectedItem;
            if(key == null)
            {
                if (string.Empty == (string)DefaultLabel.Content)
                    DefaultLabel.Content = DefaultText;
            }
            else if (key == CustomContnet)
            {
                var addFetcherWindow = this.TryFindParent<AddFetcherWindow>();
                var ctime = await addFetcherWindow.ShowInputAsync("Time Form", "Write the Time");
                if (ctime != null && TimeSpan.TryParse(ctime, out TimeSpan timeToSet))
                {
                    SelectedTime = timeToSet;
                    DefaultLabel.Content = timeToSet.ToString();
                    IntervalBox.SelectedIndex = -1;
                }
                else if (ctime != null)
                {
                    _ = await addFetcherWindow.ShowMessageAsync("Time Form", "Invalid Value!");
                }
            }
            else
            {
                SelectedTime = keyValuePairs[key];
                DefaultLabel.Content = string.Empty;
            }

        }
    }
}
