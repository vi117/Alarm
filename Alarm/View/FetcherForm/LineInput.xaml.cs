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
        static readonly DependencyProperty BackgroundFillProperty =
            DependencyProperty.Register(
                "BackgroundFill",
                typeof(Brush),
                typeof(LineInput),
                new PropertyMetadata((o, e) =>
                {
                    var form = o as LineInput;
                    form.InputBox.Background = (Brush)e.NewValue;
                }));
        static readonly DependencyProperty BackgroundTextProperty =
            DependencyProperty.Register(
                "BackgroundText",
                typeof(string),
                typeof(LineInput),
                new PropertyMetadata("default", BackgroundTextChanged));
        static readonly DependencyProperty BackgroundTextFillProperty =
            DependencyProperty.Register(
               "BackgroundTextFill",
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
        static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(LineInput),
            new PropertyMetadata("", (o, e) =>
            {
                var form = o as LineInput;
                form.InputBox.Text = (string)e.NewValue;
                if (!form.modified)
                {
                    if (form.InputBox.Text == string.Empty)
                    {
                        form.modified = false;
                        form.InputBox.Text = form.BackgroundText;
                        form.InputBox.Foreground = form.BackgroundTextFill;
                    }
                    else form.InputBox.Foreground = form.Foreground;
                }
            }));

        static private void BackgroundTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var form = o as LineInput;
            if (!form.modified)
            {
                form.InputBox.Text = (string)e.NewValue;
                form.InputBox.Foreground = form.BackgroundTextFill;
            }
        }
        static private void BackgroundTextFillChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var form = o as LineInput;
            if (!form.modified)
            {
                form.InputBox.Foreground = e.NewValue as Brush;
            }
        }

        private Storyboard WarningStoryBoard;
        private Storyboard FocusedStoryBoard;
        private Storyboard NotfocusedStoryBoard;
        private Storyboard ValidateStoryBoard;
        private bool modified = false;

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
            get => (Brush)GetValue(BackgroundTextFillProperty);
            set => SetValue(BackgroundTextFillProperty, value);
        }
        public Brush BackgroundFill
        {
            get => (Brush)GetValue(BackgroundFillProperty);
            set => SetValue(BackgroundFillProperty, value);
        }
        public VerityHandler Validation
        {
            get => GetValue(ValidationHandler) as VerityHandler;
            set => SetValue(ValidationHandler, value as VerityHandler);
        }

        public string Text {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }
        private void InputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            if (!modified)
            {
                box.Text = "";
                modified = true;
                box.Foreground = this.Foreground;
            }
            FocusedStoryBoard.Begin();
        }
        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBox;
            if (InputBox.Text == string.Empty && modified)
            {
                box.Text = BackgroundText;
                box.Foreground = BackgroundTextFill;
                modified = false;
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
            if (modified)
            {
                Text = box.Text;
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
