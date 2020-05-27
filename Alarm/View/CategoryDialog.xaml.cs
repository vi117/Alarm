using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Alarm.View
{
    /// <summary>
    /// CategoryDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CategoryDialog : MetroWindow
    {
        public CategoryDialog()
        {
            InitializeComponent();
        }
        public string getTitleName()
        {
            return TitleTextBox.Text;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = getTitleName() != String.Empty;
            e.Handled = true;
        }
    }
}
