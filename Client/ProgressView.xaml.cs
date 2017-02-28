// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HomeHub.Client
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class ProgressView : UserControl
    {
        public ProgressView()
        {
            this.InitializeComponent();
        }

        #region ProgressRing

        public ProgressRing ProgressRing
        {
            get
            {
                return (ProgressRing)GetValue(ProgressRingProperty);
            }

            set
            {
                SetValue(ProgressRingProperty, value);
            }
        }

        public static DependencyProperty ProgressRingProperty = DependencyProperty.Register("ProgressRing", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(ProgressRingPropertyChanged)));

        private static void ProgressRingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.ProgressRing = (ProgressRing)e.NewValue;
            }
        }

        #endregion ProgressRing

        #region ProgressRingDiameter

        public int ProgressRingDiameter
        {
            get
            {
                return (int)GetValue(ProgressRingDiameterProperty);
            }

            set
            {
                SetValue(ProgressRingDiameterProperty, value);
            }
        }

        public static DependencyProperty ProgressRingDiameterProperty = DependencyProperty.Register("ProgressRingDiameter", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(ProgressRingDiameterPropertyChanged)));

        private static void ProgressRingDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.ProgressRingDiameter = (int)e.NewValue;
            }
        }

        #endregion ProgressRingDiameter

        #region ProgressBar

        public ProgressBar ProgressBar
        {
            get
            {
                return (ProgressBar)GetValue(ProgressBarProperty);
            }

            set
            {
                SetValue(ProgressBarProperty, value);
            }
        }

        public static DependencyProperty ProgressBarProperty = DependencyProperty.Register("ProgressBar", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(ProgressBarPropertyChanged)));

        private static void ProgressBarPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.ProgressBar = (ProgressBar)e.NewValue;
            }
        }

        #endregion ProgressBar

        #region BlockingText

        public string BlockingText
        {
            get
            {
                return (string)GetValue(BlockingTextProperty);
            }

            set
            {
                SetValue(BlockingTextProperty, value);
            }
        }

        public static DependencyProperty BlockingTextProperty = DependencyProperty.Register("BlockingText", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(BlockingTextPropertyChanged)));

        private static void BlockingTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.BlockingText = (string)e.NewValue;
            }
        }

        #endregion BlockingText

        #region BlockingTextBlock

        public TextBlock BlockingTextBlock
        {
            get
            {
                return (TextBlock)GetValue(BlockingTextBlockProperty);
            }

            set
            {
                SetValue(BlockingTextBlockProperty, value);
            }
        }

        public static DependencyProperty BlockingTextBlockProperty = DependencyProperty.Register("BlockingTextBlock", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(BlockingTextBlockPropertyChanged)));

        private static void BlockingTextBlockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.BlockingTextBlock = (TextBlock)e.NewValue;
            }
        }

        #endregion BlockingTextBlock

        #region BlockingTextFontSize

        public int BlockingTextFontSize
        {
            get
            {
                return (int)GetValue(BlockingTextFontSizeProperty);
            }

            set
            {
                SetValue(BlockingTextFontSizeProperty, value);
            }
        }

        public static DependencyProperty BlockingTextFontSizeProperty = DependencyProperty.Register("BlockingTextFontSize", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(BlockingTextFontSizePropertyChanged)));

        private static void BlockingTextFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.BlockingTextFontSize = (int)e.NewValue;
            }
        }

        #endregion BlockingTextFontSize

        #region NonBlockingText

        public string NonBlockingText
        {
            get
            {
                return (string)GetValue(NonBlockingTextProperty);
            }

            set
            {
                SetValue(NonBlockingTextProperty, value);
            }
        }

        public static DependencyProperty NonBlockingTextProperty = DependencyProperty.Register("NonBlockingText", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(NonBlockingTextPropertyChanged)));

        private static void NonBlockingTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.NonBlockingText = (string)e.NewValue;
            }
        }

        #endregion NonBlockingText

        #region NonBlockingTextBlock

        public TextBlock NonBlockingTextBlock
        {
            get
            {
                return (TextBlock)GetValue(NonBlockingTextBlockProperty);
            }

            set
            {
                SetValue(NonBlockingTextBlockProperty, value);
            }
        }

        public static DependencyProperty NonBlockingTextBlockProperty = DependencyProperty.Register("NonBlockingTextBlock", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(NonBlockingTextBlockPropertyChanged)));

        private static void NonBlockingTextBlockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.NonBlockingTextBlock = (TextBlock)e.NewValue;
            }
        }

        #endregion NonBlockingTextBlock

        #region NonBlockingTextFontSize

        public int NonBlockingTextFontSize
        {
            get
            {
                return (int)GetValue(NonBlockingTextFontSizeProperty);
            }

            set
            {
                SetValue(NonBlockingTextFontSizeProperty, value);
            }
        }

        public static DependencyProperty NonBlockingTextFontSizeProperty = DependencyProperty.Register("NonBlockingTextFontSize", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(NonBlockingTextFontSizePropertyChanged)));

        private static void NonBlockingTextFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.NonBlockingTextFontSize = (int)e.NewValue;
            }
        }

        #endregion NonBlockingTextFontSize

        #region IsBlockingVisible

        public Visibility IsBlockingVisible
        {
            get
            {
                return (Visibility)GetValue(IsBlockingVisibleProperty);
            }

            set
            {
                SetValue(IsBlockingVisibleProperty, value);
            }
        }

        public static DependencyProperty IsBlockingVisibleProperty = DependencyProperty.Register("IsBlockingVisible", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(IsBlockingVisiblePropertyPropertyChanged)));

        private static void IsBlockingVisiblePropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.IsBlockingVisible = (Visibility)e.NewValue;
            }
        }

        #endregion IsBlockingVisible

        #region IsNonBlockingVisible

        public Visibility IsNonBlockingVisible
        {
            get
            {
                return (Visibility)GetValue(IsNonBlockingVisibleProperty);
            }

            set
            {
                SetValue(IsNonBlockingVisibleProperty, value);
            }
        }

        public static DependencyProperty IsNonBlockingVisibleProperty = DependencyProperty.Register("IsNonBlockingVisible", typeof(double), typeof(ProgressView), new PropertyMetadata(new PropertyChangedCallback(IsNonBlockingVisiblePropertyPropertyChanged)));

        private static void IsNonBlockingVisiblePropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProgressView;
            if (control != null)
            {
                control.IsNonBlockingVisible = (Visibility)e.NewValue;
            }
        }

        #endregion IsNonBlockingVisible
    }
}
