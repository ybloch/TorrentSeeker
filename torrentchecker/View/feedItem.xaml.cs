using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for feedItem.xaml
    /// </summary>
    public partial class feedItem : UserControl
    {
   
        public feedItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// event for drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dragSource = this;
            var dataObj = new DataObject(this);
            dataObj.SetData("dragSource", dataObj);
            DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Move);
        }
        /// <summary>
        /// menu event to remove movie from left list (feeds list) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Master wnd = (Master)Window.GetWindow(this);
            NewMoviesPage nmp = (NewMoviesPage)wnd.newMovieFrame.Content;
            nmp.removeFeedItem(this);
            
        }
        /// <summary>
        /// event to open menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu contextMenu = (sender as Grid).ContextMenu;
            contextMenu.PlacementTarget = sender as UIElement;
            contextMenu.IsOpen = true;
        }
        /// <summary>
        /// event - click on the image, open the image in the main screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Master wnd = (Master)Window.GetWindow(this);
            wnd.mainFrame.Navigate(new MoviePoster(circleImage));
        }

        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Master wnd = (Master)Window.GetWindow(this);
            mainPage nmp = (mainPage)wnd.mainFrame.Content;
            nmp.textBox.Text = label.Content.ToString();
        }

        private void torrentLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string torrentFile = Execute.downloadFile(torrentLinkString.Content.ToString());
            Execute.run_torrent(torrentFile);
            Thread.Sleep(7000);
            if (File.Exists(torrentFile))
                File.Delete(torrentFile);
        }
    }
}
