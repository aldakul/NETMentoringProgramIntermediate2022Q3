using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private int imgNmb;     // the number of the image currently shown
        private string link = "http://example.com";    // the URL where the currently shown ad leads to
        private readonly ImageBrush myBrush = new ImageBrush();
        private readonly BitmapImage[] _images =
        {
            new BitmapImage(new Uri("ad1.jpg", UriKind.Relative)),
            new BitmapImage(new Uri("ad2.jpg", UriKind.Relative)),
            new BitmapImage(new Uri("ad3.jpg", UriKind.Relative))
        };


        public AdWindow(Window owner)
        {
            Random rnd = new Random();
            Owner = owner;
            Width = 350;
            Height = 100;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            Title = "Support us by clicking the ads";
            Cursor = Cursors.Hand;
            ShowActivated = false;
            MouseDown += OnClick;
            
            imgNmb = rnd.Next(0, 2);
            ChangeAds(this, new EventArgs());

            // Run the timer that changes the ad's image 
            adTimer = new DispatcherTimer();
            adTimer.Interval = TimeSpan.FromSeconds(3);
            adTimer.Tick += ChangeAds;
            adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            Unsubscribe();
            base.OnClosed(e);
        } 

        public void Unsubscribe()
        {
            adTimer.Tick -= ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            myBrush.ImageSource = _images[imgNmb];
            Background = myBrush;
            imgNmb = (imgNmb+1)% _images.Length;
            //switch (imgNmb)
            //{
            //    case 1:
            //        myBrush.ImageSource =
            //            new BitmapImage(new Uri("ad1.jpg", UriKind.Relative));
            //        Background = myBrush;
            //        imgNmb++;
            //        break;
            //    case 2:
            //        myBrush.ImageSource =
            //            new BitmapImage(new Uri("ad2.jpg", UriKind.Relative));
            //        Background = myBrush;
            //        imgNmb++;
            //        break;
            //    case 3:
            //        myBrush.ImageSource =
            //            new BitmapImage(new Uri("ad3.jpg", UriKind.Relative));
            //        Background = myBrush;
            //        imgNmb = 1;
            //        break;
            //}
            
        }
    }
}