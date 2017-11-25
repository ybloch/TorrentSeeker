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
using TorrentChecker.View;

namespace TorrentChecker
{
    /// <summary>
    /// Interaction logic for movieItem.xaml
    /// </summary>
    public partial class movieItem : UserControl
    {
        public static event customHandler deleteButton_Clicked;
        public delegate void customHandler(object sender);

        public movieItem()
        {
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteButton_Clicked != null)
                deleteButton_Clicked(this);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dragSource = this;
            var dataObj = new DataObject(this);
            dataObj.SetData("dragSource", dataObj);
            DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Move);
        }

        private void movieName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Master wnd = (Master)Window.GetWindow(this);
            mainPage nmp = (mainPage)wnd.mainFrame.Content;
            nmp.textBox.Text = movieName.Text;
        }
    }
}
