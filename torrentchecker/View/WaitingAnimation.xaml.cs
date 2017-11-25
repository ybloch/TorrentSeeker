using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TorrentChecker.View
{
    /// <summary>
    /// Interaction logic for WaitingAnimation.xaml
    /// </summary>
    public partial class WaitingAnimation : UserControl
    {
        ColorAnimation animation = new ColorAnimation();
        int counter = 0;
        public WaitingAnimation()
        {
            InitializeComponent();
            //startAnimation();
           // this.Loaded+= WaitingAnimationPage_loaded;
            this.Loaded+= button_Click;

        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), new Thickness(500, 0, -500, 0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate1_Completed);

            ellipse_4.BeginAnimation(Ellipse.MarginProperty, animate);
        }
        private void animate8_Completed(object sender, EventArgs e)
        {

            Master wnd = (Master)Window.GetWindow(this);
            if (wnd != null)
            {
                mainPage nmp = (mainPage)wnd.mainFrame.Content;
                if (nmp != null)
                {
                    animation.BeginAnimation(Ellipse.MarginProperty, null);
                }
            }
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), new Thickness(500, 0, -500, 0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate1_Completed);
            ellipse_4.BeginAnimation(Ellipse.MarginProperty, animate);
         
        }


        private void animate7_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(-500, 0, 500, 0), new Thickness(0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate8_Completed);

            ellipse_1.BeginAnimation(Ellipse.MarginProperty, animate);
            
        }
        private void animate6_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(-500, 0, 500, 0), new Thickness(0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate7_Completed);

            ellipse_2.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private void animate5_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(-500, 0, 500, 0), new Thickness(0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate6_Completed);

            ellipse_3.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private void animate4_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(-500, 0, 500, 0), new Thickness(0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate5_Completed);

            ellipse_4.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private void animate3_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), new Thickness(500, 0, -500, 0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate4_Completed);

            ellipse_1.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private void animate2_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), new Thickness(500, 0, -500, 0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate3_Completed);

            ellipse_2.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private void animate1_Completed(object sender, EventArgs e)
        {
            ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), new Thickness(500, 0, -500, 0), TimeSpan.FromMilliseconds(300), FillBehavior.HoldEnd);
            animate.Completed += new EventHandler(animate2_Completed);

            ellipse_3.BeginAnimation(Ellipse.MarginProperty, animate);

        }
        private async void WaitingAnimationPage_loaded(object sender, RoutedEventArgs e)
        {
            await startslideAnimation();
        }
        private async Task  startslideAnimation()
        {
            var sb = new Storyboard();
            var slideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = new Thickness(this.Width, 0, -this.Width, 0),
                To = new Thickness(0),
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
           // Storyboard.SetTargetName(slideAnimation, "ellipse_1");
            sb.Children.Add(slideAnimation);
            sb.Begin(this);
            await Task.Delay(5000);
        } 
        private void startAnimation()
        {
            ellipse_1.Visibility = Visibility.Hidden;
           // ellipse_2.Visibility = Visibility.Hidden;
           // ellipse_3.Visibility = Visibility.Hidden;
            ellipse_4.Visibility = Visibility.Hidden;

            Color color = (Color)ColorConverter.ConvertFromString("#FF7CD14C");
            animation.From = color;
            animation.To = Colors.Transparent;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            //animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.Completed += moveToNextEllipse;
            ColorAnimation animation2 = new ColorAnimation();
            Color color2 = (Color)ColorConverter.ConvertFromString("#FF7CD14C");
            animation2.From = Colors.Transparent; 
            animation2.To = color;
            animation2.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation2.RepeatBehavior = RepeatBehavior.Forever;

            ellipse_1.Visibility = Visibility.Visible;
            this.ellipse_1.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);


            //this.ellipse_1.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
            //Thread.Sleep(50);

            //this.ellipse_2.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
            //Thread.Sleep(50);

            //this.ellipse_3.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
            //Thread.Sleep(50);

            //this.ellipse_4.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
            //Thread.Sleep(50);

        }
        private void moveToNextEllipse(object sender, EventArgs e)
        {
            counter++;
            if ((counter % 4) == 0)
            {
                ellipse_1.Visibility = Visibility.Visible;
                this.ellipse_1.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else if ((counter % 4) == 1)
            {
                ellipse_2.Visibility = Visibility.Visible;
                this.ellipse_2.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else if ((counter % 4) == 2)
            {
                ellipse_3.Visibility = Visibility.Visible;
                this.ellipse_3.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else if ((counter % 4) == 3)
            {
                ellipse_4.Visibility = Visibility.Visible;
                this.ellipse_4.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }

        }
    }
}
