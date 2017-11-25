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
using Awesomium.Core;

namespace TorrentChecker.View
{
    /// <summary>
    /// Interaction logic for webPage.xaml
    /// </summary>
    public partial class webPage : Page
    {
        public webPage(string movieName)
        {
            InitializeComponent();
            web.Source = string.Format(@"http://www.imdb.com/find?ref_=nv_sr_fn&q="+movieName).ToUri();
        }
        public void closePage()
        {
            this.Visibility = Visibility.Hidden;
        }
        public void changeWebUrl(string movieName)
        {
            web.Source = String.Format(@"http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movieName).ToUri();
        }
    }
}
