using Alarm.ViewModels;
using Model;
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
using ViewModel;

namespace Alarm.View.FetcherForm
{
    /// <summary>
    /// RSSForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RSSForm : FetcherFormControl
    {
        public RSSForm()
        {
            InitializeComponent();
        }

        override public Model.Fetcher GetFetcher()
        {
            return new RSSFetcher(URLContent.Text) { Interval = IntervalBox.SelectedTime };
        }
        override public string GetTitle()
        {
            return FetcherTitle.Text;
        }
        private bool UrlValid(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
