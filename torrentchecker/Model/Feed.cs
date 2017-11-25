
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using TorrentChecker.View;

namespace TorrentChecker
{
    class Feed
    {
       public String subject { get; set; }
       public String summary { get; set; }
       public String image { get; set; }
       public String torrent { get; set; }

        public static List<Feed> getFeeds(string url, bool isTV = false)
        {
            try
            {
                List<Feed> feeds = new List<Feed>();
                if (!isTV)
                    addFromXML(feeds);
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                List<SyndicationItem> listFeeds = removeAllItemsExistingInFile(feed.Items);
                foreach (SyndicationItem item in listFeeds)
                {
                    if(isTV)
                        addTVShowsFeed(feeds, item);
                    else
                        addFeed(feeds, item);
                }
                return feeds;

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "torrent checker Error message", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return null;

        }
      
        private static void addFeed(List<Feed> feeds, SyndicationItem item)
        {
            //if (alreadyExistinFile(item.Title.Text))
            //    return;
            Feed f = new Feed();
            f.subject = item.Title.Text;
            string[] str = item.Summary.Text.Split('>');
            f.summary = str[2];
            f.image = str[5];

            addToXML(item.Title.Text);

            feeds.Add(f);
            
        }
        private static void addTVShowsFeed(List<Feed> feeds, SyndicationItem item)
        {
            //if (alreadyExistinFile(item.Title.Text))
            //    return;
            Feed f = new Feed();
            f.torrent = "";
            if (item.Title != null)
            {
                f.subject = item.Title.Text;
                foreach (var link in item.Links)
                {
                    if(link.Uri.ToString().ToLower().EndsWith(".torrent"))
                    {
                        f.torrent = link.Uri.ToString();
                        break;
                    }
                }
                if (item.Summary != null)
                {
                    string[] str = item.Summary.Text.Split('>');
                    f.image = str[5];
                }
                else
                {
                   f.image = Execute.getImage(item.Title.Text);
                }
                addToXML(item.Title.Text, f.torrent);
                feeds.Add(f);
            }

        }
        private static void addFromXML(List<Feed> feeds)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\FeedsInfo.xml"))
                return;
            XElement xmlFile = XElement.Load("FeedsInfo.xml");
            foreach (var item in xmlFile.Elements())
            {
                //foreach (var f in feeds) //always empty..
                //{
                //    if (item.Element("name").Value == f.subject)
                //        break;
                //}
                Feed feed = new Feed() { subject = item.Element("name").Value, image = item.Element("image").Value,
                                            torrent = item.Element("torrent").Value};
                feeds.Add(feed);
             

            }
            return;
        }

        private static void addToXML(string name, string torrent = "")
        {
            
            string img = Execute.getImage(name);
            if (img == null)
                img = "/TorrentChecker;component/images/add-icon.png";
            XElement xmlFile;
            XElement newElement = new XElement("Feed",
                new XElement("name", name),
                new XElement("image", img),
                new XElement("torrent", torrent));
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
        public static bool alreadyExistinFile(string name)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\FeedsInfo.xml"))
                return false;
            XElement xmlFile = XElement.Load("FeedsInfo.xml");
            foreach (var item in xmlFile.Elements())
            {
                if (item.Element("name").Value == name)
                    return true;
            }
            return false;

        }
        public static List<SyndicationItem> removeAllItemsExistingInFile(IEnumerable<SyndicationItem> feeds)
        {
            List<SyndicationItem> resFeeds = new List<SyndicationItem>();
            foreach (var feed in feeds)
            {
                resFeeds.Add(feed);
            }
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\FeedsInfo.xml"))
                return resFeeds;     
            XElement xmlFile = XElement.Load("FeedsInfo.xml");         
            foreach (var feed in feeds)
            {
                foreach (var item in xmlFile.Elements())
                {
                    if (item.Element("name").Value == feed.Title.Text)
                    {
                        resFeeds.Remove(feed);
                        break;
                    }
                }

            }
            return resFeeds;
        }

    }
}
