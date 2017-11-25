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

namespace TorrentChecker.View
{
    /// <summary>
    /// Interaction logic for MoviePoster.xaml
    /// </summary>
    public partial class MoviePoster : Page
    {
        public MoviePoster(ImageBrush img)
        {
            InitializeComponent();
            posterContainer.Source = img.ImageSource;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }
    }
}
