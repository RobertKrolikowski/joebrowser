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

namespace JoeBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Website website = new Website();
        public MainWindow()
        {
            Task t = new Task (website.LastNumber);
            t.Start();          
            if (t.Status == TaskStatus.RanToCompletion)
            {
               Task<BitmapImage> bitmap = website.LoadContent();
            }
            InitializeComponent();


        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            website.ChangeCurrentNumber(-1);
            website.ChangeUrl();
            website.LoadContent();
            if (website.GetImgLink() != null)
                image.Source = new BitmapImage(new Uri(website.GetImgLink()));
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            website.ChangeCurrentNumber(1);
            website.ChangeUrl();
            website.LoadContent();
            if(website.GetImgLink() != null)
                image.Source = new BitmapImage(new Uri(website.GetImgLink()));
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            website.SaveImage();
        }
    }
}
