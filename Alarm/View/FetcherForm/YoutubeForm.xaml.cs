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
    [FetcherForm("Youtube", typeof(YoutubeFetcher))]
    public partial class YoutubeForm : FetcherFormControl
    {
        override public string FetcherName
        {
            get => FetcherTitle.Text;
            set => FetcherTitle.Text = value;
        }

        public YoutubeForm()
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
            var fetcher = f as YoutubeFetcher;
            Feed.Text = fetcher.ChannelId;
            IntervalBox.SelectedTime = fetcher.Interval;
        }

        override public Model.Fetcher CreateFetcher()
        {
            var f = YoutubeFetcher.IsYoutubeURL(Feed.Text) ?
                YoutubeFetcher.FromURL(Feed.Text) :
                YoutubeFetcher.FromChannelId(Feed.Text);
            f.Interval = IntervalBox.SelectedTime;
            return f;
        }
    }
}
