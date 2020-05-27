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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm.View.FetcherForm
{
    /// <summary>
    /// TimeComboBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimeComboBox : UserControl
    {
        Dictionary<string, TimeSpan> keyValuePairs = new Dictionary<string, TimeSpan>()
        {
            ["1 minute"] = TimeSpan.FromMinutes(1),
            ["5 minutes"] = TimeSpan.FromMinutes(5),
            ["30 minutes"] = TimeSpan.FromMinutes(30),
            ["1 hour"] = TimeSpan.FromHours(1),
            ["3 hours"] = TimeSpan.FromHours(3),
        };
        public TimeComboBox()
        {
            InitializeComponent();
            IntervalBox.ItemsSource = keyValuePairs.Keys;
            IntervalBox.SelectedIndex = 2;
        }
        public TimeSpan SelectedTime
        {
            get => keyValuePairs[(string)IntervalBox.SelectedItem];
        }
    }
}
