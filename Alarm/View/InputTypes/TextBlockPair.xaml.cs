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

namespace Alarm.View.InputTypes
{
    /// <summary>
    /// TextBlockPair.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TextBlockPair : UserControl
    {
        public TextBlockPair()
        {
            InitializeComponent();
        }
        public TextBlockPair(string name,string defaultValue) : this()
        {
            attrName.Text = name;
            attrValue.Text = defaultValue;
            
        }
        public string NameText {
            get => attrName.Text;
            set => attrName.Text = value;
        }
        public string NameValueText
        {
            get => attrValue.Text;
            set => attrValue.Text = value;
        }
        public double NameFontSize
        {
            get => attrName.FontSize;
            set => attrName.FontSize = value;
        }
        public double ValueFontSize {
            get => attrValue.FontSize;
            set => attrValue.FontSize = value;
        }
    }
}
