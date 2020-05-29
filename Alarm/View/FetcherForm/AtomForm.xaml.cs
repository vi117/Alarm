using Model;
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
    /// AtomForm.xaml에 대한 상호 작용 논리
    /// </summary>
    [FetcherForm("Atom",typeof(AtomFetcher))]
    public partial class AtomForm : FetcherFormControl
    {
        public AtomForm()
        {
            InitializeComponent();
        }
        public AtomForm(AtomFetcher atomFetcher)
        {
            URLContent.Text = atomFetcher.Uri;
        }
        override public Fetcher CreateFetcher()
        {
            return new AtomFetcher(URLContent.Text) { Interval = IntervalBox.SelectedTime };
        }
        public override void SetFromFetcher(Fetcher f)
        {
            var fetcher = (AtomFetcher)f;
            URLContent.Text = fetcher.Uri;
            IntervalBox.SelectedTime = fetcher.Interval;
        }
        public override string FetcherName {
            get => FetcherTitle.Text;
            set => FetcherTitle.Text = value;
        }
        private bool UrlValid(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
