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
        public ChargingView()
        {
            InitializeComponent();
            LoadImages();
            CreateAnimation();
        }
        private void LoadImages()
        {
            
            string[] path = new string[5];
            string[] keypath = new string[5];

            path[0] = "pack://application:,,,/Resources/xaml/Phone/UnpluggedPhone.xaml";
            path[1] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery1.xaml";
            path[2] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery12.xaml";
            path[3] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery123.xaml";
            path[4] = "pack://application:,,,/Resources/xaml/Phone/PluggedPhoneBattery1234.xaml";

            keypath[0] = "UnpluggedPhone";
            keypath[1] = "PluggedPhoneBattery1";
            keypath[2] = "PluggedPhoneBattery12";
            keypath[3] = "PluggedPhoneBattery123";
            keypath[4] = "PluggedPhoneBattery1234";

            for(int i = 0; i <=4; i++)
            {
                var drawingImage = TryFindResource(keypath[i]) as DrawingImage;
                if (drawingImage != null)
                {
                    var image = new Image
                    {
                        Source = drawingImage,
                        Stretch = System.Windows.Media.Stretch.Uniform,
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
        

        private void CreateAnimation()
        {
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            double delay = 0;

            for (int i = 0; i < _images.Count; i++)
            {
                var fadeIn = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.FromSeconds(delay),
                    Duration = TimeSpan.FromSeconds(1)
                };

                var fadeOut = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.FromSeconds(delay + 1),
                    Duration = TimeSpan.FromSeconds(0.1)
                };

                Storyboard.SetTarget(fadeIn, _images[i]);
                Storyboard.SetTarget(fadeOut, _images[i]);
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
                Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));

                _storyboard.Children.Add(fadeIn);
                _storyboard.Children.Add(fadeOut);

                delay += 1; // każda klatka co 1s
            }

            _storyboard.Begin();
        }
    }
}
