using MahApps.Metro.Controls;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm.View
{
    /// <summary>
    /// ContentListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ContentListView : Page
    {
        public ContentListView()
        {
            InitializeComponent();
        }

        private void ContentListViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            /*var frame = this.TryFindParent<Frame>();
            MaxWidth = frame.ActualWidth;*/
        }

        private void ContentListViewPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*var frame = this.TryFindParent<Frame>();
            this.ListBox.Width = frame.ActualWidth;*/
        }
    }
}
