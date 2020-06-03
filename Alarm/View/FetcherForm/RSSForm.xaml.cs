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
    [FetcherForm("RSS",typeof(RSSFetcher))]
    public partial class RSSForm : FetcherFormControl
    {
        public RSSForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set from Fetcher
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        /// <param name="f">Fetcher</param>
        override public void SetFromFetcher(Fetcher f)
        {
            var fetcher = (RSSFetcher)f;
            URLContent.Text = fetcher.URL;
            IntervalBox.SelectedTime = fetcher.Interval;
        }
        override public Model.Fetcher CreateFetcher()
        {
            return new RSSFetcher(URLContent.Text) { Interval = IntervalBox.SelectedTime };
        }
        override public string FetcherName
        {
            get => FetcherTitle.Text;
            set => FetcherTitle.Text = value;
        }
        private bool UrlValid(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
