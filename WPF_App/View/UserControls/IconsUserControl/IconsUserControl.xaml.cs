using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DeskManagementStand_App.View.UserControls.IconsUserControl
{
    public partial class IconsUserControl : UserControl
    {
        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(IconsUserControl), new PropertyMetadata(string.Empty));

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(string), typeof(IconsUserControl), new PropertyMetadata(string.Empty));

        public string Command
        {
            get { return (string)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public IconsUserControl()
        {
            InitializeComponent();
            Loaded += IconsUserControl_Loaded;
            IconGrid.SizeChanged += AdjustIconsSize;
        }
        private void IconsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the image source to the icon path
            if (!string.IsNullOrEmpty(Icon))
            {
                var drawingimage = TryFindResource(Icon) as DrawingImage;
                if (drawingimage != null)
                {

                    var image = new Image
                    {
                        Source = drawingimage,
                        //Margin = new Thickness(5, 5, 5, 5),
                        Width = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Stretch = Stretch.Uniform,
                    };
                    IconGrid.Children.Clear(); // Clear previous images
                    IconGrid.Children.Add(image);
                }
            }

            IconGrid.Children.Add(IconButton);
            IconButton.Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
            IconButton.Width = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
            // Założenie: DataContext jest ustawiony i zawiera właściwość OpenConnectionSettingsCommand
            if (!string.IsNullOrEmpty(Command))
            { 
            var binding = new Binding(Command)
            {
                Mode = BindingMode.OneWay
            };

            IconButton.SetBinding(Button.CommandProperty, binding);
            }
        }

        private void AdjustIconsSize(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Icon))
            {
                var drawingimage = TryFindResource(Icon) as DrawingImage;
                if (drawingimage != null)
                {

                    var image = new Image
                    {
                        Source = drawingimage,
                        //Margin = new Thickness(5, 5, 5, 5),
                        Width = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Stretch = Stretch.Uniform,
                    };
                    IconGrid.Children.Clear(); // Clear previous images
                    IconGrid.Children.Add(image);
                }
            }

            IconGrid.Children.Add(IconButton);
            IconButton.Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
            IconButton.Width = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
        }
    }
}
