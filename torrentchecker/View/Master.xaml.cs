using System.Windows;
using System.Timers;
using System.Windows.Input;
using System.Net;
using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace TorrentChecker.View
{
    /// <summary>
    /// Interaction logic for Master.xaml
    /// </summary>
    public partial class Master : Window
    {
        private static Timer aTimer;
        WellcomePage wellcomePage;
        public Master()
        {

            InitializeComponent();
            wellcomePage = new WellcomePage();
            MasterFrame.NavigationService.Navigate(wellcomePage);
            
            // Create a timer with a 5 second interval.
            aTimer = new Timer(60000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            //Set the Interval to 5 seconds (5000 milliseconds).
            aTimer.Interval = 7000;
            aTimer.Enabled = true;
            aTimer.Start();
            if(!isConnectToNetwork())
            {
                MessageBox.Show("There no Internet Connection, check connectivity and try again", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();

            }
            //string newVersion = getNewVersion();
            //if (newVersion != null)
            //{
            //    try
            //    {
            //        replaceProgram(newVersion);
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show("Error in software updating\nERROR: " + e.Message.ToString(), "Updating Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}

        }

        private void replaceProgram(string fileToRun)
        {
            if (!fileToRun.ToLower().Contains("torrentseeker") || !fileToRun.ToLower().EndsWith(".exe"))
                throw new Exception("incurrect file update");
            Process p = new Process();
            p.StartInfo.FileName = fileToRun;
            p.Start();
            Application.Current.Shutdown();
        }

        private string getNewVersion()
        {
            try
            {
                string myVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string[] apps = Directory.GetFiles(@"c:\TorrentSeekerRelease");
                foreach (var item in apps)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(item);
                    string versionInDir = versionInfo.ProductVersion;
                    Version first = new Version(myVersion);
                    Version second = new Version(versionInDir);
                    if (first < second)
                        return item;
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error checking whether the software is updated\nERROR: "+ e.Message.ToString(), "Updating Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static bool isConnectToNetwork()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                aTimer.Stop();
                mainFrame.NavigationService.Navigate(new mainPage());
                newMovieFrame.Navigate(new NewMoviesPage());

                if (MasterFrame.NavigationService.CanGoBack)
                {
                    MasterFrame.NavigationService.GoBack();
                }
                MasterFrame.NavigationService.Navigate(new WaitingPage());
                MasterFrame.Visibility = Visibility.Hidden;
            });
    

        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    //Handle double-click
                    if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                        if (minMaxRec.MinHeight == minMaxRec.Height && minMaxRec.MinWidth == minMaxRec.Width)
                        {
                            minMaxRec.Height = minMaxRec.Height * 2;
                            minMaxRec.Width = minMaxRec.Width * 2;
                            minMaxRec.VerticalAlignment = VerticalAlignment.Center;

                        }
                    }
                    else if (App.Current.MainWindow.WindowState == WindowState.Normal)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Maximized;
                        if (minMaxRec.MaxHeight == minMaxRec.Height && minMaxRec.MaxWidth == minMaxRec.Width)
                        {

                            minMaxRec.Height = minMaxRec.Height / 2;
                            minMaxRec.Width = minMaxRec.Width / 2;
                            minMaxRec.VerticalAlignment = VerticalAlignment.Bottom;
                        }
                    }
                }
                else
                    App.Current.MainWindow.DragMove();
            }
            catch{    }
        }

        private void minimaize_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void maximaize_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
                if (minMaxRec.MinHeight == minMaxRec.Height && minMaxRec.MinWidth == minMaxRec.Width)
                {
                    minMaxRec.Height = minMaxRec.Height * 2;
                    minMaxRec.Width = minMaxRec.Width * 2;
                    minMaxRec.VerticalAlignment = VerticalAlignment.Center;

                }
            }
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
                if (minMaxRec.MaxHeight == minMaxRec.Height && minMaxRec.MaxWidth == minMaxRec.Width)
                {

                    minMaxRec.Height = minMaxRec.Height / 2;
                    minMaxRec.Width = minMaxRec.Width / 2;
                    minMaxRec.VerticalAlignment = VerticalAlignment.Bottom;
                }

            }
        }


        private void close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        #region resize window
        private void Top_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Height > this.MinHeight)
            {
                this.Height -= e.VerticalChange;
                this.Top += e.VerticalChange;
            }
            else
            {
                this.Height = this.MinHeight + 4;
                TopThumb.ReleaseMouseCapture();
            }

        }

        private void Btm_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Height > this.MinHeight)
            {
                this.Height += e.VerticalChange;
            }
            else
            {
                this.Height = this.MinHeight + 4;
                BtmThumb.ReleaseMouseCapture();
            }
        }

        private void Rgt_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width > this.MinWidth)
            {
                this.Width += e.HorizontalChange;
            }
            else
            {
                this.Width = this.MinWidth + 4;
                RgtThumb.ReleaseMouseCapture();
            }
        }

        private void Lft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width > this.MinWidth)
            {
                this.Width -= e.HorizontalChange;
                this.Left += e.HorizontalChange;
            }
            else
            {
                this.Width = this.MinWidth + 4;
                LftThumb.ReleaseMouseCapture();
            }
        }
        #endregion

    
    }
}

//         if (File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
//            {
//                XElement xmlFile = XElement.Load("moviesList.xml");
//                foreach (XElement m in xmlFile.Elements())
//                {
//                    loadCheckList(m.Element("image").Value, m.Element("name").Value);
//                }
//            }
//            movieItem.deleteButton_Clicked += new movieItem.customHandler(removeItem);
//            System.Collections.Generic.List<Feed> feeds = Feed.getFeeds();
//            if (feeds != null)
//            {
//                foreach (var item in feeds)
//                {
//                    feedItem fi = new feedItem();
//fi.label.Content = item.subject;
//                    fi.label.PreviewMouseLeftButtonDown += itemnewMovies_FocusChanged;
//                    fi.rightArrow.PreviewMouseLeftButtonDown += rightArrowClicked;
//                    string img = item.image;
//                    if (img != null && img != "/TorrentChecker;component/images/add-icon.png")
//                    {
//                        var uri = new Uri(img);
//var bitmap = new BitmapImage(uri);
//fi.circleImage.ImageSource = bitmap;
//                    }
//                    listnewMovies.Items.Add(fi);
//                }
//            }
//            goBackbtm.Visibility = Visibility.Hidden;
//            goBackbtm.PreviewMouseLeftButtonDown += goBackClicked;
//            changeScreen();

//private void loadCheckList(string image, string name)
//{

//    ListBoxItem item = new ListBoxItem();
//    movieItem movie_item = new movieItem();
//    movie_item.movieName.Text = name;
//    // item.Content = movie_item;
//    //   item.FontSize = 12;
//    item.PreviewMouseRightButtonDown += Item_PreviewMouseRightButtonDown; ;
//    //  listView.Items.Add(new ListBoxItem().Content = movieName);
//    //  string img = Execute.getImage(movieName);
//    string img = image;
//    if (img != null && !img.Contains(@"/TorrentChecker;component/images/"))
//    {
//        var uri = new Uri(img);
//        var bitmap = new BitmapImage(uri);
//        movie_item.movieImage.ImageSource = bitmap;
//    }

//    listView.Items.Add(movie_item);

//}

//private void Item_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
//{
//    if (sender is ListBoxItem)
//    {
//        ListBoxItem item = (ListBoxItem)sender;
//        item.ContextMenuOpening += Item_ContextMenuOpening;
//    }

//}

//private void Item_ContextMenuOpening(object sender, ContextMenuEventArgs e)
//{

//}

//private /*async*/ void checkmyList()
//{
//    XElement xmlFile = XElement.Load("moviesList.xml");
//    if (xmlFile.Elements().Count() == 0)
//    {
//        System.Windows.MessageBox.Show("no movies in the list, please add one first", "torrent checker message", MessageBoxButton.OK, MessageBoxImage.Information);
//        return;
//    }
//    foreach (var m in xmlFile.Elements())
//    {

//        string torrent = /*await*/ Execute.run_cmd(m.Element("name").Value);
//        if (torrent != "-1")
//            Execute.run_torrent(torrent);
//    }
//}
//private BackgroundWorker _backgroundWorker;
//private void checkList_Click(object sender, RoutedEventArgs e)
//{
//    //_backgroundWorker = new BackgroundWorker();
//    //waitingGif.Visibility = Visibility.Visible;
//    //_backgroundWorker.DoWork += new DoWorkEventHandler();
//    //_backgroundWorker.WorkerReportsProgress = true;
//    //_backgroundWorker.RunWorkerAsync();

//    checkmyList();

//    //     waitingGif.Visibility = Visibility.Hidden;


//}



//private void textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//{

//    if (textBox.Text == "add to search list:") { textBox.Text = ""; }
//}

//private void addButton_Click(object sender, RoutedEventArgs e)
//{
//    string img = Execute.getImage(textBox.Text);
//    if (img == null)
//        img = "/TorrentChecker;component/images/movieIcon2.png";
//    XElement xmlFile;
//    XElement newElement = new XElement("movie",
//        new XElement("name", textBox.Text),
//        new XElement("image", img));
//    if (!File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
//    {
//        xmlFile = new XElement("movies", newElement);
//    }
//    else
//    {
//        xmlFile = XElement.Load("moviesList.xml");
//        xmlFile.Add(newElement);

//    }

//    xmlFile.Save("moviesList.xml");
//    loadCheckList(img, textBox.Text);
//    textBox.Text = " ";

//}

//public void removeItem(object sender)
//{
//    if (!(sender is movieItem))
//        return;
//    movieItem movieItemSender = sender as movieItem;
//    string movieName = movieItemSender.movieName.Text;
//    XElement xmlFile = XElement.Load("moviesList.xml");
//    (from m in xmlFile.Elements()
//     where m.Element("name").Value == movieName
//     select m).FirstOrDefault().Remove();
//    xmlFile.Save("moviesList.xml");
//    movieItem temp = null;
//    foreach (movieItem item in listView.Items)
//    {
//        if (item.movieName.Text == movieName)
//        {
//            temp = item;
//            break;
//        }
//    }
//    if (temp != null)
//        listView.Items.Remove(temp);

//}

//private void itemnewMovies_FocusChanged(object sender, RoutedEventArgs e)
//{
//    Label fi = sender as Label;
//    textBox.Text = fi.Content.ToString();
//}
//private void rightArrowClicked_old(object sender, RoutedEventArgs e)
//{
//    BlockArrow ba = sender as BlockArrow;
//    Grid p = ba.Parent as Grid;
//    feedItem fi = p.Parent as feedItem;
//    this.containWebPage.NavigationService.Navigate(getWebPage(fi.label.Content.ToString()));
//    goBackbtm.Visibility = Visibility.Visible;
//}//
//private void rightArrowClicked(object sender, RoutedEventArgs e)
//{
//    Image ba = sender as Image;
//    Grid p = ba.Parent as Grid;
//    feedItem fi = p.Parent as feedItem;
//    this.containWebPage.NavigationService.Navigate(getWebPage(fi.label.Content.ToString()));
//    goBackbtm.Visibility = Visibility.Visible;
//}//
//private void goBackClicked(object sender, RoutedEventArgs e)
//{
//    if (wp != null)
//        wp.closePage();
//    goBackbtm.Visibility = Visibility.Hidden;
//}
//private static webPage getWebPage(string movieName)
//{
//    if (wp != null)
//    {
//        wp.changeWebUrl(movieName);
//        wp.Visibility = Visibility.Visible;
//        return wp;
//    }
//    else
//    {
//        wp = new webPage(movieName);
//        wp.Visibility = Visibility.Visible;
//        return wp;
//    }


//}


//private void listView_DragOver(object sender, DragEventArgs e)
//{
//    if (e.Data.GetDataPresent(typeof(ListViewItem)))
//    {
//        e.Effects = DragDropEffects.Move;
//    }
//}

//private void listView_Drop(object sender, DragEventArgs e)
//{
//    if (e.Data.GetDataPresent(typeof(feedItem)))
//    {
//        var item = (feedItem)e.Data.GetData(typeof(feedItem));

//        addFeedItemtoCheckList(item);

//    }

//}
//private void listnewMovies_Drop(object sender, DragEventArgs e)
//{
//    if (e.Data.GetDataPresent(typeof(movieItem)))
//    {
//        var item = (movieItem)e.Data.GetData(typeof(movieItem));

//        addMovieItemtoNewMovieList(item);
//        removeItem(item);

//    }
//}
//private void addMovieItemtoNewMovieList(movieItem mi)
//{
//    feedItem fi = new feedItem();
//    fi.label.Content = mi.movieName.Text;
//    //fi.circleImage = mi.movieImage;
//    fi.label.PreviewMouseLeftButtonDown += itemnewMovies_FocusChanged;
//    fi.rightArrow.PreviewMouseLeftButtonDown += rightArrowClicked;
//    string img = mi.movieImage.ImageSource.ToString();
//    if (img != null && img != "/TorrentChecker;component/images/add-icon.png")
//    {
//        var uri = new Uri(img);
//        var bitmap = new BitmapImage(uri);
//        fi.circleImage.ImageSource = bitmap;
//    }
//    listnewMovies.Items.Add(fi);

//    // add to xml 
//    if (Feed.alreadyExistinFile(fi.label.Content.ToString()))
//        return;
//    XElement xmlFile;
//    XElement newElement = new XElement("Feed",
//        new XElement("name", fi.label.Content),
//        new XElement("image", img));
//    if (!File.Exists(Directory.GetCurrentDirectory() + "\\FeedsInfo.xml"))
//    {
//        xmlFile = new XElement("Feeds", newElement);
//    }
//    else
//    {
//        xmlFile = XElement.Load("FeedsInfo.xml");
//        xmlFile.Add(newElement);

//    }

//    xmlFile.Save("FeedsInfo.xml");
//}

//private void addFeedItemtoCheckList(feedItem fi)
//{
//    movieItem mi = new movieItem();
//    ListBoxItem item = new ListBoxItem();
//    movieItem movie_item = new movieItem();
//    movie_item.movieName.Text = fi.label.Content.ToString();
//    item.PreviewMouseRightButtonDown += Item_PreviewMouseRightButtonDown;
//    // movie_item.movieImage = fi.circleImage;
//    if (fi.circleImage != null && fi.circleImage.ImageSource.ToString() != "/TorrentChecker;component/images/add-icon.png")
//    {
//        var uri = new Uri(fi.circleImage.ImageSource.ToString());
//        var bitmap = new BitmapImage(uri);
//        movie_item.movieImage.ImageSource = bitmap;
//    }
//    listView.Items.Add(movie_item);

//    // add to xml
//    XElement xmlFile;
//    XElement newElement = new XElement("movie",
//       new XElement("name", movie_item.movieName.Text),
//       new XElement("image", movie_item.movieImage.ImageSource));
//    if (!File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
//    {
//        xmlFile = new XElement("movies", newElement);
//    }
//    else
//    {
//        xmlFile = XElement.Load("moviesList.xml");
//        xmlFile.Add(newElement);

//    }

//    xmlFile.Save("moviesList.xml");
//}

//private void waitingGif_MediaEnded(object sender, RoutedEventArgs e)
//{
//    waitingGif.Position = new TimeSpan(0, 0, 1);
//    waitingGif.Play();
//}

//private void waitingGif_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
//{
//    if (waitingGif.Visibility == Visibility.Visible)
//        waitingGif.Play();
//    else
//        waitingGif.Stop();


//}

