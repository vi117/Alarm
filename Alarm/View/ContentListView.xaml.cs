using Alarm.Language;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Papago;
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
using TheArtOfDev.HtmlRenderer.WPF;

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
        async Task<bool> TranslateHtmlLabel(HtmlAgilityPack.HtmlNode node,Papago.PapagoGlue translator)
        {
            if (!node.HasChildNodes)
            {
                node.InnerHtml = await translator.Translate(node.InnerHtml);
            }
            else
            {
                foreach (var child in node.ChildNodes)
                    await TranslateHtmlLabel(child, translator);
            }
            return true;
        }
        async public void TranslateSelected()
        {
            try
            {
                var item = ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.SelectedItem);
                if (item == null)
                {
                    await this.TryFindParent<MainWindow>().ShowMessageAsync(
                        this.GetText("AlertTitle"),
                        this.GetText("SelectFirstMessage"));
                    return;
                }
                var title = item.FindChild<TextBlock>("TitleLabel");
                var summary = item.FindChild<HtmlLabel>("SummaryLabel");
                var pass = App.Setting.PapagoApiPass;
                var translator = new Papago.PapagoGlue(pass);
                var titleResult = translator.Translate(title.Text);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(summary.Text);
                await TranslateHtmlLabel(doc.DocumentNode, translator);
                var summaryResult = doc.DocumentNode.InnerHtml;
                title.Text = await titleResult;
                summary.Text = summaryResult;
            }
            catch(NotAuthorizedException _)
            {
                await this.TryFindParent<MainWindow>().ShowMessageAsync(
                        this.GetText("AlertTitle"),
                        this.GetText("FailedTranslateMessage"));
                return;
            }
        }
    }
}
