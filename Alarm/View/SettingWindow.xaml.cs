using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Alarm.View
{
    /// <summary>
    /// SettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingWindow : MahApps.Metro.Controls.MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
            DataContext = App.Setting;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.Save(Setting.DefaultPath);
            this.DialogResult = true;
            e.Handled = true;
        }
    }
}
