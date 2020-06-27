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
        private Setting settingOrigin;
        public SettingWindow()
        {
            InitializeComponent();
            DataContext = App.Setting;
            settingOrigin = new Setting();
            App.Setting.CopyTo(settingOrigin);
            PapagoAPISecretInput.Password = App.Setting.PapagoApiSecret;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.Save(Setting.DefaultPath);
            this.DialogResult = true;
            e.Handled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            settingOrigin.CopyTo(App.Setting);
            DialogResult = false;
            e.Handled = true;
        }

        private void PapagoAPISecretInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            App.Setting.PapagoApiSecret = PapagoAPISecretInput.Password;
        }
    }
}
