using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TorrentChecker
{
    class Execute
    {
        //open the magnet link with the associated application
        //TODO: open the .torrent with any other program - done. ---- great job!!
        public static void run_torrent(string torrent)
        {
            Process.Start(torrent);

            //Process p = new Process();
            //p.StartInfo = new ProcessStartInfo(@"C:\Users\yinon\AppData\Roaming\uTorrent\uTorrent.exe")
            //{
            //    WorkingDirectory = @"C:\Users\yinon\AppData\Roaming\uTorrent",
            //    Arguments = "uTorrent.exe" + " " + torrent,
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};
            //p.Start();
        }


        //send GET request to piratebay.org with the movie name
        public static string downloadLinkFromWeb(string movieName)
        {

            string url = @"https://thepiratebay.org/search/" + movieName; // i think here we need to replace " " with "_"

            //crate new webclient and download the html
            List<string> linkList = new List<string>();
            try
            {
                string downloadString = loadLink(url);
                linkList = ParseHtml(downloadString, string.Format("class=\"detName\""), string.Format("using magnet\">"));

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "ERROR MESSAGE", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            //parse the html and get List<string> of torrent class
            //the class contain the movie name , and the movie magnet link 
            //parm1 = the html file
            //parm2 = the string in the start of "detName" calss
            //parm3 = the string in the end of "detName" class 

            //get the movie name and magnet link from the class


            selectDialog sd = new selectDialog(linkList);
            if (sd.ShowDialog() == true)
            {
                return sd.selectedLink;
            }
            else
            {
                if (sd.selectedLink == "canceled" || sd.selectedLink==null || sd.selectedLink=="")
                    return "-1";
                else
                    return getMovieMagnetLink(sd.selectedLink);

            }

    }

        public static string loadLink(string url)
        {
            string downloadString = "";
            using (WebClient client = new WebClient())
            {
                downloadString = client.DownloadString(url);

            }
            return downloadString;
        }

        public static string downloadFile(string url)
        {
            string fileLocation = Directory.GetCurrentDirectory() + "\\Temp\\TorrentLink.torrent";
            if (!Directory.Exists(Path.GetDirectoryName(fileLocation)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, fileLocation);
            }
            return fileLocation;
        }

        public static string getLinkFromWeb(string url, string start, string end)
        {
            try
            {
                WebClient client = new WebClient();
                string downloadString = client.DownloadString(url);
                List<string> listOfLinks = new List<string>();
                listOfLinks = ParseHtml(downloadString, string.Format(start), string.Format(end));
                if (listOfLinks.Count > 0)
                {
                    string[] onlyLink = listOfLinks[0].Split('"');
                    if (onlyLink.Length > 3)
                        return onlyLink[3];
                    else
                        return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public static string getImage(string movieName)
        {
          //  if (isInStore(movieName))
         //       return getImageFromStore(movieName);
            string linkOfMovieWeb = Execute.getLinkFromWeb(@"http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movieName, "class=\"result_text\"", movieName);
            return Execute.getImageFromWeb(@"http://www.imdb.com/" + linkOfMovieWeb, "img alt=\"" + movieName + " Poster\"", "itemprop=\"image\"");
        }

        private static string getImageFromStore(string movieName)
        {
            //https://drive.google.com/drive/folders/0BwR0Sh-X5OJGR1A2Uk45OUw0elE?usp=sharing
            try
            {

                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(@"https://drive.google.com/drive/folders/0BwR0Sh-X5OJGR1A2Uk45OUw0elE?usp=sharing", Directory.GetCurrentDirectory());
                    // return new FileInfo(path);
                   // var file = FileInfo(Directory.GetCurrentDirectory());
                    return null;
                }

            }
            catch (WebException)
            {
                return null;
            }
        }

        private static bool isInStore(string movieName)
        {
            //https://drive.google.com/drive/folders/0BwR0Sh-X5OJGR1A2Uk45OUw0elE?usp=sharing
            try
            {

                using (var webClient = new WebClient())
                {
                   webClient.DownloadFile(@"https://drive.google.com/drive/folders/0BwR0Sh-X5OJGR1A2Uk45OUw0elE?usp=sharing", @"c:\TempTemp\text.txt");
                    string[] files = Directory.GetFiles(@"c:\TempTemp");
                    // return new FileInfo(path);
                    // var file = FileInfo(Directory.GetCurrentDirectory());
                    return false;
                }

            }
            catch (WebException e)
            {
                return false;
            }
        }

        private static string getImageFromWeb(string url, string start, string end)
        {
            using (WebClient client = new WebClient())
            {
                string downloadString = client.DownloadString(url);
                List<string> listOfLinks = new List<string>();
                listOfLinks = ParseHtml(downloadString, string.Format(start), string.Format(end));
                if (listOfLinks.Count > 0)
                {
                    string[] onlyLink = listOfLinks[0].Split('"');
                    if (onlyLink.Length > 5)
                        return onlyLink[5];
                    else
                        return null;
                }
                else
                    return null;
            }
        }
        //parse all the html file
        private static List<string> ParseHtml(string strSource, string strStart, string strEnd)
        {
            List<string> resList = new List<string>(); 

            int Start, End;

            while (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                try
                {

                    Start = strSource.IndexOf(strStart, 0);//(strStart);
                    End = strSource.IndexOf(strEnd, Start) + strEnd.Length;

                    resList.Add(strSource.Substring(Start, End - Start));

                    strSource = strSource.Substring(End, strSource.Length - End); //cut after the class
                }
                catch (Exception e)
                {
                    break;
                }


            }

            return resList;

        }

        //parse the "detName" calss (contain movie name and magnet link inside from the HTML format)
        //return the full display name (not 'only' the name)
        public static string getMovieName(string classString)
        {

            var Start = classString.IndexOf(string.Format("title="),0) + string.Format("title=").Length + 2; // 2 is for =\
            var end = classString.IndexOf(string.Format("</a>"), 0);

            return classString.Substring(Start , end - Start); 
        }

        //parse the "detName" calss (contain movie name and magnet link inside from the HTML format)
        //return the magnet link to the movie 
        private static string getMovieMagnetLink(string classString)
        {
            classString = classString.Substring(classString.IndexOf(string.Format("</a>")));            // cut the start of the calss

            var Start = classString.IndexOf(string.Format("href="), 0) + string.Format("href=").Length + 1; // 2 is for =
            var end = classString.IndexOf(string.Format("&"), 0);

            return classString.Substring(Start, end - Start); ;
        }
    }
}
