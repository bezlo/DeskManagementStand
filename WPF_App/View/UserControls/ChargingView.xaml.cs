using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DeskManagementStand_App.View.UserControls
{

    public partial class ChargingView : UserControl
    {
        private List<Image> _images = new();
        private Storyboard _storyboard;

        public static readonly DependencyProperty UnpluggedDeviceProperty =
        DependencyProperty.Register("UnpluggedDevice", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PluggedDeviceProperty =
        DependencyProperty.Register("PluggedDevice", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PluggedDevice1Property =
        DependencyProperty.Register("PluggedDevice1", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PluggedDevice2Property =
        DependencyProperty.Register("PluggedDevice2", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PluggedDevice3Property =
        DependencyProperty.Register("PluggedDevice3", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty PluggedDevice4Property =
        DependencyProperty.Register("PluggedDevice4", typeof(string), typeof(ChargingView), new PropertyMetadata(string.Empty));

        public string UnpluggedDevice
        {
            get { return (string)GetValue(UnpluggedDeviceProperty); }
            set { SetValue(UnpluggedDeviceProperty, value); }
        }
        public string PluggedDevice
        {
            get { return (string)GetValue(PluggedDeviceProperty); }
            set { SetValue(PluggedDeviceProperty, value); }
        }
        public string PluggedDevice1
        {
            get => (string)GetValue(PluggedDevice1Property);
            set => SetValue(PluggedDevice1Property, value);
        }
        public string PluggedDevice2
        {
            get => (string)GetValue(PluggedDevice2Property);
            set => SetValue(PluggedDevice2Property, value);
        }
        public string PluggedDevice3
        {
            get => (string)GetValue(PluggedDevice3Property);
            set => SetValue(PluggedDevice3Property, value);
        }
        public string PluggedDevice4
        {
            get => (string)GetValue(PluggedDevice4Property);
            set => SetValue(PluggedDevice4Property, value);
        }

        public ChargingView()

        {
            InitializeComponent();
            this.Loaded += ChargingView_Loaded;
        }
        private void ChargingView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImages();
            //CreateAnimation_SmoothTransition();
            CreateAnimation_StackTransition();
        }
        private void LoadImages()
        {
            ChargingGrid.Children.Clear(); // Clear previous images
            _images.Clear(); // Clear the list of images
            //string[] path = new string[5];
            string[] keypath = new string[6];

            //path[0] = "pack://application:,,,/Resources/xaml/Phone/UnpluggedPhone.xaml";
            //path[1] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery1.xaml";
            //path[2] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery12.xaml";
            //path[3] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery123.xaml";
            //path[4] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery1234.xaml";

            keypath[0] = UnpluggedDevice;
            keypath[1] = PluggedDevice;
            keypath[2] = PluggedDevice1;
            keypath[3] = PluggedDevice2;
            keypath[4] = PluggedDevice3;
            keypath[5] = PluggedDevice4;

            double width = ChargingGrid.ActualWidth - 5; //* 0.95;
            double height = ChargingGrid.ActualHeight - 5;// * 0.95;
            double sizeImage = Math.Min(width, height);

            for (int i = 0; i <=5; i++)
            {
                var drawingImage = TryFindResource(keypath[i]) as DrawingImage;
                if (drawingImage != null)
                {
                    var image = new Image
                    {
                        Source = drawingImage,
                        //Stretch = System.Windows.Media.Stretch.Uniform,
                        Width = width,
                        Height = height,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(2, 0, 0, 0),
                        Opacity = 0
                    };
                    _images.Add(image);
                    ChargingGrid.Children.Add(image);
                }
            }
            //Uri uri = new Uri(path[0]);
            //BitmapImage bitmap = new BitmapImage(uri);

            //var image = new Image
            //{
            //    Source = bitmap,
            //    Stretch = System.Windows.Media.Stretch.Uniform,
            //    Opacity = 1
            //};
            //for (int i = 0; i <= 4; i++)
            //{
            //    var image = new Image
            //    {
            //        //Source = new BitmapImage(new Uri(path[i])),
            //        Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery1234.xaml")),
            //        Stretch = System.Windows.Media.Stretch.Uniform,
            //        Opacity = 0
            //    };
        }
        

        private void CreateAnimation_SmoothTransition()
        {
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            double delay = 0;

            for (int i = 1; i < _images.Count; i++)
            {
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.FromSeconds(delay-0.4),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };

                var fadeOut = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.FromSeconds(delay + 1),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };

                Storyboard.SetTarget(fadeIn, _images[i]);
                Storyboard.SetTarget(fadeOut, _images[i]);
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
                Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));

                _storyboard.Children.Add(fadeIn);
                _storyboard.Children.Add(fadeOut);

                delay += 2; // fps
            }
            _storyboard.Begin();
        }
        private void CreateAnimation_StackTransition()
        {
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            double delay = 0;
            for (int i = 1; i < _images.Count; i++)
            {
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.FromSeconds(delay - 0.4),
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };

                
                Storyboard.SetTarget(fadeIn, _images[i]);
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));

                _storyboard.Children.Add(fadeIn);

                delay += 2; // fps
            }
            _storyboard.Begin();
        }

        private void ChargingGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LoadImages();
            CreateAnimation_StackTransition();
        }
    }
}
