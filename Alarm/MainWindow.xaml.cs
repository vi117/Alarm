using Publisher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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


namespace Alarm
{
    public class ViewModel : INotifyPropertyChanged{
        private TreeViewModel treeView;
        private object page;
        public ViewModel()
        {
            //test data
            //if (DesignerProperties.GetIsInDesignMode())
            {
                treeView = new TreeViewModel();
                {
                    var sc = new CategoryItem("Science");
                    sc.SiteModels = new ObservableCollection<SiteModel>();
                    var siteA = new SiteModel("A");
                    {
                        var doc = new Document();
                        doc.Title = "News1";
                        doc.Summary = "News1 Summary";
                        doc.Date = DateTime.Now;
                        doc.GUID = "1";
                        siteA.Add(new DocumentView(doc));
                        doc = new Document();
                        doc.Title = "News2";
                        doc.Summary = "News2 Summary";
                        doc.Date = DateTime.Now;
                        doc.GUID = "2";
                        siteA.Add(new DocumentView(doc));
                    }
                    sc.SiteModels.Add(siteA);
                    sc.SiteModels.Add(new SiteModel("B"));
                    sc.SiteModels.Add(new SiteModel("C"));
                    treeView.Add(sc);
                }
                {
                    var yt = new CategoryItem("Youtube");
                    yt.SiteModels = new ObservableCollection<SiteModel>();
                    yt.SiteModels.Add(new SiteModel("A"));
                    yt.SiteModels.Add(new SiteModel("B"));
                    treeView.Add(yt);
                }
            }
        }
        public TreeViewModel TreeView
        {
            get => treeView;
            set {
                treeView = value;
                OnPropertyChanged("TreeView");
            }
        }
        public object Page
        {
            get => page;
            set
            {
                page = value;
                OnPropertyChanged("Page");
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Trace.WriteLine(e.NewValue);
            IAlertPage page = e.NewValue as IAlertPage;
            Navigate(page);
        }
        private void Navigate(IAlertPage page)
        {
            var view = PageFactory.Generate(page.ValidPageName);
            view.DataContext = page;
            PageFrame.Navigate(view);
        }
    }
}
