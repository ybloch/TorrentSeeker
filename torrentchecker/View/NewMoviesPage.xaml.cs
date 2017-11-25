using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace TorrentChecker.View
{
        /// <summary>
        /// Interaction logic for NewMoviesPage.xaml
        /// </summary>
        public partial class NewMoviesPage : Page
        {
            private static webPage wp { get; set; }

            public NewMoviesPage()
        {
            InitializeComponent();
            Task t = new Task(initPage);
            t.Start();
            //goBackbtm.Visibility = Visibility.Hidden;
            //        goBackbtm.PreviewMouseLeftButtonDown += goBackClicked;
            //changeScreen();
        }

        private void initPage()            
        {
           
            System.Collections.Generic.List<Feed> feeds = Feed.getFeeds(Properties.Resources.NewMoviesURL);
            feeds.AddRange(Feed.getFeeds(Properties.Resources.NewTVshowsURL, true));
            App.Current.Dispatcher.Invoke(delegate
            {
                if (feeds != null)
                {
                    foreach (var item in feeds)
                    {
                        feedItem fi = new feedItem();
                        fi.label.Content = item.subject;
                        //fi.label.PreviewMouseLeftButtonDown += itemnewMovies_FocusChanged;
                        fi.rightArrow.PreviewMouseLeftButtonDown += rightArrowClicked;
                        string img = item.image;
                        if (img != null && img != "/TorrentChecker;component/images/add-icon.png")
                        {
                            var uri = new Uri(img);
                            var bitmap = new BitmapImage(uri);
                            fi.circleImage.ImageSource = bitmap;
                        }
                        fi.torrentLinkString.Content = item.torrent;
                        if (string.IsNullOrWhiteSpace(item.torrent))
                        {
                            fi.torrentLink.Visibility = Visibility.Collapsed;
                        }
                        listnewMovies.Items.Add(fi);
                    }
                }
            });
        }

        private void rightArrowClicked_old(object sender, RoutedEventArgs e)
            {
                BlockArrow ba = sender as BlockArrow;
                Grid p = ba.Parent as Grid;
                feedItem fi = p.Parent as feedItem;
                Master wnd = (Master)Window.GetWindow(this);
                wnd.mainFrame.NavigationService.Navigate(getWebPage(fi.label.Content.ToString()));
                //goBackbtm.Visibility = Visibility.Visible;
            }//
            private void rightArrowClicked(object sender, RoutedEventArgs e)
            {
                Image ba = sender as Image;
                Grid p = ba.Parent as Grid;
                while (!(p.Parent is feedItem))
                    p = p.Parent as Grid;
                feedItem fi = p.Parent as feedItem;
                Master wnd = (Master)Window.GetWindow(this);
                wnd.mainFrame.NavigationService.Navigate(getWebPage(fi.label.Content.ToString()));
             //   goBackbtm.Visibility = Visibility.Visible;
            }//
            private void goBackClicked(object sender, RoutedEventArgs e)
            {
                if (wp != null)
                    wp.closePage();
              //  goBackbtm.Visibility = Visibility.Hidden;
            }
            private static webPage getWebPage(string movieName)
            {
                if (wp != null)
                {
                    wp.changeWebUrl(movieName);
                    wp.Visibility = Visibility.Visible;
                    return wp;
                }
                else
                {
                    wp = new webPage(movieName);
                    wp.Visibility = Visibility.Visible;
                    return wp;
                }
            }
            //private void itemnewMovies_FocusChanged(object sender, RoutedEventArgs e)
            //{
            //    Label fi = sender as Label;
            //    textBox.Text = fi.Content.ToString();
                 //}
        private void listnewMovies_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(movieItem)))
            {
                var item = (movieItem)e.Data.GetData(typeof(movieItem));

                addMovieItemtoNewMovieList(item);
                removeItem(item);

            }
        }
        public void removeFeedItem(object sender)
        {
            if (!(sender is feedItem))
                return;
            feedItem feedItemSender = sender as feedItem;
            string movieName = feedItemSender.label.Content.ToString();
            XElement xmlFile = XElement.Load("FeedsInfo.xml");
            (from m in xmlFile.Elements()
             where m.Element("name").Value == movieName
             select m).FirstOrDefault().Remove();
            xmlFile.Save("FeedsInfo.xml");
            feedItem temp = null;
            foreach (feedItem item in listnewMovies.Items)
            {
                if (item.label.Content.ToString() == movieName)
                {
                    temp = item;
                    break;
                }
            }
            if (temp != null)
                listnewMovies.Items.Remove(temp);

        }
        private void addMovieItemtoNewMovieList(movieItem mi)
        {
            feedItem fi = new feedItem();
            fi.label.Content = mi.movieName.Text;
            //fi.circleImage = mi.movieImage;
            //fi.label.PreviewMouseLeftButtonDown += itemnewMovies_FocusChanged;
            fi.rightArrow.PreviewMouseLeftButtonDown += rightArrowClicked;
            string img = mi.movieImage.ImageSource.ToString();
            if (img != null && img != "/TorrentChecker;component/images/add-icon.png")
            {
                var uri = new Uri(img);
                var bitmap = new BitmapImage(uri);
                fi.circleImage.ImageSource = bitmap;
            }
            listnewMovies.Items.Add(fi);

            // add to xml 
            if (Feed.alreadyExistinFile(fi.label.Content.ToString()))
                return;
            XElement xmlFile;
            XElement newElement = new XElement("Feed",
                new XElement("name", fi.label.Content),
                new XElement("image", img),
                new XElement("torrent", fi.torrentLinkString.Content));
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\FeedsInfo.xml"))
            {
                xmlFile = new XElement("Feeds", newElement);
            }
            else
            {
                xmlFile = XElement.Load("FeedsInfo.xml");
                xmlFile.Add(newElement);

            }

            xmlFile.Save("FeedsInfo.xml");
        }
        public void removeItem(object sender)
        {
            if (!(sender is movieItem))
                return;
            movieItem movieItemSender = sender as movieItem;
            string movieName = movieItemSender.movieName.Text;
            XElement xmlFile = XElement.Load("moviesList.xml");
            (from m in xmlFile.Elements()
             where m.Element("name").Value == movieName
             select m).FirstOrDefault().Remove();
            xmlFile.Save("moviesList.xml");
            movieItem temp = null;
            Master wnd = (Master)Window.GetWindow(this);
            mainPage mp = (mainPage)wnd.mainFrame.Content;
            foreach (movieItem item in mp.listView.Items)
            {
                if (item.movieName.Text == movieName)
                {
                    temp = item;
                    break;
                }
            }
            if (temp != null)
                mp.listView.Items.Remove(temp);

        }
    }
}
