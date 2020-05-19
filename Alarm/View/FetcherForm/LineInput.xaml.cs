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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm.View.FetcherForm
{
    /// <summary>
    /// LineInput.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LineInput : UserControl
    {
        static readonly DependencyProperty BackgroundTextProperty =
            DependencyProperty.Register(
                nameof(BackgroundTextProperty),
                typeof(string),
                typeof(LineInput),
                new PropertyMetadata("default", BackgroundTextChanged));
        static readonly DependencyProperty BackgroundTextFillProperty =
            DependencyProperty.Register(
                nameof(BackgroundTextFillProperty),
                typeof(Brush),
                typeof(LineInput),
                new PropertyMetadata(Brushes.Gray, BackgroundTextFillChanged));

        public delegate bool VerityHandler(string value);

        static readonly DependencyProperty ValidationHandler =
           DependencyProperty.Register(
               nameof(ValidationHandler),
               typeof(VerityHandler),
               typeof(LineInput),
               new PropertyMetadata(new VerityHandler((s) => true)));
        static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register(
                nameof(TextForegroundProperty),
                typeof(Brush),
                typeof(LineInput),
                new PropertyMetadata(Brushes.Black));

        static private void BackgroundTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var form = o as LineInput;
            if (form.InputBox.Text == string.Empty || form.InputBox.Text == (string)e.OldValue)
            {
                form.InputBox.Text = (string)e.NewValue;
                form.InputBox.Foreground = form.BackgroundTextFill;
            }
        }
        static private void BackgroundTextFillChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var form = o as LineInput;
            if (form.InputBox.Text == form.BackgroundText)
            {
                form.InputBox.Foreground = e.NewValue as Brush;
            }
        }

        private Storyboard WarningStoryBoard;
        private Storyboard FocusedStoryBoard;
        private Storyboard NotfocusedStoryBoard;
        private Storyboard ValidateStoryBoard;

        public LineInput()
        {
            InitializeComponent();
            WarningStoryBoard = FindResource("WarningSB") as Storyboard;
            FocusedStoryBoard = FindResource("FocusedSB") as Storyboard;
            NotfocusedStoryBoard = FindResource("NotFocusedSB") as Storyboard;
            ValidateStoryBoard = FindResource("ValidateSB") as Storyboard;
            InputBox.GotFocus += InputBox_GotFocus;
            InputBox.LostFocus += InputBox_LostFocus;
        }

        public string BackgroundText
        {
            get => GetValue(BackgroundTextProperty) as string;
            set => SetValue(BackgroundTextProperty, value as string);
        }
        public Brush BackgroundTextFill
        {
            get => GetValue(BackgroundTextFillProperty) as Brush;
            set => SetValue(BackgroundTextProperty, value as Brush);
        }
        public Brush TextForeground
        {
            get => GetValue(TextForegroundProperty) as Brush;
            set => SetValue(TextForegroundProperty, value as Brush);
        }

        public VerityHandler Validation
        {
            get => GetValue(ValidationHandler) as VerityHandler;
            set => SetValue(ValidationHandler, value as VerityHandler);
        }
        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            if (box.Text == BackgroundText)
            {
                box.Text = "";
                box.Foreground = TextForeground;
            }
            FocusedStoryBoard.Begin();
        }
        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            if (InputBox.Text == string.Empty)
            {
                box.Text = BackgroundText;
                box.Foreground = BackgroundTextFill;
                NotfocusedStoryBoard.Begin();
            }
            else
            {
                if (Validation.Invoke(box.Text))
                {
                    ValidateStoryBoard.Begin();
                }
                else
                {
                    WarningStoryBoard.Begin();
                }
            }
        }
        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            if (box.Text != BackgroundText)
            {
                if (Validation.Invoke(box.Text))
                {
                    ValidateStoryBoard.Begin();
                }
                else
                {
                    WarningStoryBoard.Begin();
                }
            }
        }
    }
}
