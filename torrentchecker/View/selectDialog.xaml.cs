
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace TorrentChecker
{
    /// <summary>
    /// Interaction logic for selectDialog.xaml
    /// </summary>
    public partial class selectDialog : Window
    {
        public string selectedLink { get; set; }

        public selectDialog(List<string> links)
        {
            InitializeComponent();

            if(links != null)
            {
                foreach (var item in links)
                {
                    ListViewItem tempItem = new ListViewItem();
                    string[] longName = Execute.getMovieName(item).Split('>');
                    if(longName.Length>=2)
                        tempItem.Content = longName[1];
                    tempItem.Uid = item;
                    listView_selectQuality.Items.Add(tempItem);
                }
            }
            
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            selectedLink = "canceled";
            this.Close();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)listView_selectQuality.SelectedItem;
            if (lvi != null  && lvi.Uid != null)
                selectedLink = lvi.Uid;
            this.Close();
        }
     //   public string getLink() { return selectedLink; }

    }
}
