using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TorrentChecker.Model;

namespace TorrentChecker.View
{
    /// <summary>
    /// Interaction logic for mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        string addedMovieName { get; set; }
        /// <summary>
        /// constractor - load all the items from xml file
        /// </summary>
        public mainPage()
        {
            InitializeComponent();
            if (File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
            {
                XElement xmlFile = XElement.Load("moviesList.xml");
                foreach (XElement m in xmlFile.Elements())
                {
                    loadCheckList(m.Element("image").Value, m.Element("name").Value);
                }
            }
            movieItem.deleteButton_Clicked += new movieItem.customHandler(removeItem);
            textBox.TextChanged += new TextChangedEventHandler(on_textChange);
      
        }



        /// <summary>
        /// load the items from xml file to listbox
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        private void loadCheckList(string image, string name)
        {

            ListBoxItem item = new ListBoxItem();
            movieItem movie_item = new movieItem();
            movie_item.movieName.Text = name;

            string img = image;
            // if the image is not the defult image
            if (img != null && !img.Contains(@"/TorrentChecker;component/images/"))
            {
                var uri = new Uri(img);
                var bitmap = new BitmapImage(uri);
                movie_item.movieImage.ImageSource = bitmap;
            }

            listView.Items.Add(movie_item);

        }
        /// <summary>
        /// checkmyList - try to download all checked movies in the list.
        /// also run torrent program on the local pc.
        /// </summary>
        private void checkmyList()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                XElement xmlFile = XElement.Load("moviesList.xml");
                if (xmlFile.Elements().Count() == 0 || listView.Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("no movies in the list, please add one first", "Torrent Seeker Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                bool noItemsSelected = true;
                foreach (var item in listView.Items)
                {
                    movieItem movieitem = item as movieItem;
                    if(movieitem.checkbox.IsChecked==true)
                    {
                        noItemsSelected = false;
                        string torrent = Execute.downloadLinkFromWeb(movieitem.movieName.Text);
                        if (torrent != "-1")
                            Execute.run_torrent(torrent);
                    }
                }
                if(noItemsSelected)
                    MessageBox.Show("No movies selected from the list, please select at least one", "Torrent Seeker Message", MessageBoxButton.OK, MessageBoxImage.Information);

                stopAnimation();

            });
        }

       /// <summary>
       /// download button event
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void checkList_Click(object sender, RoutedEventArgs e)
        {
            lbSuggestion.Visibility = Visibility.Collapsed;
            startAnimation();
            Task t = new Task(checkmyList);
            t.Start();

        }
        /// <summary>
        /// start wating animation
        /// </summary>
        private void startAnimation()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                this.gridAllPage.Opacity = 0.2;
                this.waitingAnim.Visibility = Visibility.Visible;
                this.waitingAnimGrid.Visibility = Visibility.Visible;
            });


        }
        private void stopAnimation()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                this.gridAllPage.Opacity = 1;
                this.waitingAnim.Visibility = Visibility.Hidden;
                this.waitingAnimGrid.Visibility = Visibility.Collapsed;
            });
        }

        /// <summary>
        /// add buttom event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            lbSuggestion.Visibility = Visibility.Collapsed;
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return;
            startAnimation();
            addedMovieName = textBox.Text;
            Task t = new Task(addMovie);
            t.Start();
            
            //startAnimation();
            //Task t = new Task(stopAnimation);
            //t.Start();

        }
        /// <summary>
        /// add the movie to listbox, xml file and find image for him
        /// </summary>
        private void addMovie()
        {
            string img = null;
            img = Execute.getImage(addedMovieName);
            if (img == null)
                img = "/TorrentChecker;component/images/movieIcon2.png";
            XElement xmlFile;
            XElement newElement = new XElement("movie",
                new XElement("name", addedMovieName),
                new XElement("image", img));
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
            {
                xmlFile = new XElement("movies", newElement);
            }
            else
            {
                xmlFile = XElement.Load("moviesList.xml");
                xmlFile.Add(newElement);

            }

            xmlFile.Save("moviesList.xml");
            App.Current.Dispatcher.Invoke(delegate
            {           
                loadCheckList(img, textBox.Text);
                textBox.Text = " ";
                stopAnimation();
            }); 
        }
        /// <summary>
        /// delete movie item from xml file and from listbox
        /// </summary>
        /// <param name="sender"></param>
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
            foreach (movieItem item in listView.Items)
            {
                if (item.movieName.Text == movieName)
                {
                    temp = item;
                    break;
                }
            }
            if (temp != null)
                listView.Items.Remove(temp);

        }
    
        /// <summary>
        /// for drag and drop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }
        /// <summary>
        /// drop event, add feed item to movie listbox, convert feedItem to movieItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(feedItem)))
            {
                var item = (feedItem)e.Data.GetData(typeof(feedItem));

                addFeedItemtoCheckList(item);

            }

        }

        /// <summary>
        /// convert feedItem to movieItem, add him to xml file and to listbox
        /// </summary>
        /// <param name="fi"></param>
        private void addFeedItemtoCheckList(feedItem fi)
        {
            movieItem mi = new movieItem();
            ListBoxItem item = new ListBoxItem();
            movieItem movie_item = new movieItem();
            movie_item.movieName.Text = fi.label.Content.ToString();
            if (fi.circleImage != null && fi.circleImage.ImageSource.ToString() != "/TorrentChecker;component/images/add-icon.png")
            {
                var uri = new Uri(fi.circleImage.ImageSource.ToString());
                var bitmap = new BitmapImage(uri);
                movie_item.movieImage.ImageSource = bitmap;
            }
            listView.Items.Add(movie_item);

            // add to xml
            XElement xmlFile;
            XElement newElement = new XElement("movie",
               new XElement("name", movie_item.movieName.Text),
               new XElement("image", movie_item.movieImage.ImageSource),
               new XElement("torrent", movie_item.torrentLinkString.Content));
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\moviesList.xml"))
            {
                xmlFile = new XElement("movies", newElement);
            }
            else
            {
                xmlFile = XElement.Load("moviesList.xml");
                xmlFile.Add(newElement);

            }

            xmlFile.Save("moviesList.xml");
        }

        private async void on_textChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                return;
            }
            string typedText = textBox.Text;
            List<GoogleSuggestion> suggestionList = new List<GoogleSuggestion>();
            suggestionList.Clear();
            SearchSuggestionsAPI searchSuggestionsAPI = new SearchSuggestionsAPI();
            suggestionList = await searchSuggestionsAPI.GetSearchSuggestions(typedText);
            if (suggestionList.Count > 0)
            {
                lbSuggestion.ItemsSource = suggestionList.Select(x => x.ToString()).ToList();
                lbSuggestion.Visibility = Visibility.Visible;
            }
            else if (typedText.Equals(""))
            {
                lbSuggestion.ItemsSource = null;
                lbSuggestion.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbSuggestion.ItemsSource = null;
                lbSuggestion.Visibility = Visibility.Collapsed;
            }
        }
        private void lbSuggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox)
                return;
            if (lbSuggestion.ItemsSource != null)
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                textBox.TextChanged -= new TextChangedEventHandler(on_textChange);
                if (lbSuggestion.SelectedIndex != -1)
                {
                    textBox.Text = lbSuggestion.SelectedItem.ToString();
                }
                textBox.TextChanged += new TextChangedEventHandler(on_textChange);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Down)
                if (lbSuggestion.Items.Count > 0 && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    lbSuggestion.Visibility = Visibility.Visible;
                    lbSuggestion.Focus();
                    lbSuggestion.SelectedIndex = 0;
                    //this.lbSuggestion.Items[0].Focus();
                }
        }

        private void lbSuggestion_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                lbSuggestion.Visibility = Visibility.Visible;
                if (e.IsDown && e.Key == Key.Up && lbSuggestion.SelectedIndex - 1 > -1)
                {
                    lbSuggestion.SelectedIndex--;
                }
                if (e.IsDown && e.Key == Key.Down && lbSuggestion.SelectedIndex + 1 < lbSuggestion.Items.Count)
                {
                    lbSuggestion.SelectedIndex++;
                }
                if (e.IsDown && e.Key == Key.Enter)
                {
                    ListBox tempSender = sender as ListBox;
                    int current = lbSuggestion.Items.IndexOf(tempSender.Items.CurrentItem);
                    textBox.Text = lbSuggestion.SelectedItems[current].ToString();
                    lbSuggestion.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void lbSuggestion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBox tempSender = sender as ListBox;
            int current = lbSuggestion.Items.IndexOf(tempSender.Items.CurrentItem);
            textBox.Text = lbSuggestion.SelectedItems[current].ToString();
            lbSuggestion.Visibility = Visibility.Collapsed;
        }

    }
}
