using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FishTank.Controls
{
    /// <summary>
    /// Interaction logic for FishLevelOne.xaml
    /// </summary>
    public partial class FishLevelOne : UserControl
    {
        #region Variables

        public List<BitmapImage> leftPics = new List<BitmapImage>();
        public List<BitmapImage> rightPics = new List<BitmapImage>();

        public List<BitmapImage> blueLeftPics = new List<BitmapImage>();
        public List<BitmapImage> blueRightPics = new List<BitmapImage>();

        public List<BitmapImage> RedLeftPics = new List<BitmapImage>();
        public List<BitmapImage> RedRightPics = new List<BitmapImage>();

        private DispatcherTimer clock = new DispatcherTimer();
        private DispatcherTimer blueClock = new DispatcherTimer();
        private DispatcherTimer redClock = new DispatcherTimer();
        private DispatcherTimer bubbleClock = new DispatcherTimer();

        public bool up = true, OKDrop = false, left = true, leftBlue = false, leftRed = true;
        public double tp = 0.0, fromTop = 45, SpeedUp = 0.0;
        public double fromLeftBlue = 0.0, fromTopBlue = 110;
        public double tpRed = 200.0, fromTopRed = 230;
        public double bub1Top = 0.0, bub2Top = 0.0, bub3Top = 0.0;
        public int speedCount = 0, bubbleCount = 0, ctr = 0;
        private System.Windows.Controls.Image caughtFish;
        private System.Windows.Point mousePosition;

        #endregion

        public FishLevelOne()
        {
            InitializeComponent();
            leftPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\GrnLeft.png", 20);
            rightPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\GrnRight.png", 20);

            blueLeftPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\BlueLeft.png", 20);
            blueRightPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\BlueRight.png", 20);

            RedLeftPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\RedLeft.png", 20);
            RedRightPics = GetPics("D:\\temp\\GoneFishing\\GoneFishing\\Images\\RedRight.png", 20);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"D:\\temp\\FishTank\\FishTank\\Images\\space.jpg", UriKind.Relative));
            cnvFishTank.Background = imageBrush;
            PrepPics();
            imgBubble1.Visibility = Visibility.Hidden;
            imgBubble2.Visibility = Visibility.Hidden;
            imgBubble3.Visibility = Visibility.Hidden;
            clock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            clock.Tick += clock_Tick;
            Canvas.SetTop(fishImage, fromTop);
            clock.Start();

            blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            blueClock.Tick += blueClock_Tick;
            fromLeftBlue = cnvFishTank.ActualWidth - 55;
            Canvas.SetTop(blueFishImage, fromTopBlue);
            Canvas.SetLeft(blueFishImage, fromLeftBlue);
            blueClock.Start();

            redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 36);
            redClock.Tick += redClock_Tick;
            Canvas.SetTop(redFishImage, fromTopRed);
            Canvas.SetLeft(redFishImage, tpRed);
            redClock.Start();

            bubbleClock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            bubbleClock.Tick += bubbleClock_Tick;
           
        }

        #region Fish Tank Events

        void redClock_Tick(object sender, EventArgs e)
        {
            if (leftRed)
            {
                if (SpeedUp > tpRed)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            else
            {
                if (SpeedUp > 0 && tpRed > SpeedUp)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            if (speedCount > 40) { SpeedUp = 0.0; }

            fromTopRed = Canvas.GetTop(redFishImage);
            tpRed = Canvas.GetLeft(redFishImage);

            //double wl = Canvas.ActualHeightProperty(fishImage);
            if (leftRed)
            {
                tpRed += 3;
            }
            else tpRed -= 3;

            Canvas.SetTop(redFishImage, fromTopRed);
            Canvas.SetLeft(redFishImage, tpRed);

            double widthCanvas = cnvFishTank.ActualWidth - fishImage.ActualWidth;
            if (tpRed >= widthCanvas)
            {
                leftRed = false;
            }
            else if (tpRed < 3.0)
            {
                leftRed = true;
            }
        }

        void blueClock_Tick(object sender, EventArgs e)
        {
            if (leftBlue)
            {
                if (SpeedUp > fromLeftBlue)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            else
            {
                if (SpeedUp > 0 && fromLeftBlue > SpeedUp)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            if (speedCount > 40) { SpeedUp = 0.0; }

            fromTopBlue = Canvas.GetTop(blueFishImage);
            fromLeftBlue = Canvas.GetLeft(blueFishImage);

            if (leftBlue)
            {
                fromLeftBlue += 3;
            }
            else fromLeftBlue -= 3;

            Canvas.SetTop(blueFishImage, fromTopBlue);
            Canvas.SetLeft(blueFishImage, fromLeftBlue);

            double widthCanvas = cnvFishTank.ActualWidth - fishImage.ActualWidth;
            if (fromLeftBlue >= widthCanvas)
            {
                leftBlue = false;
            }
            else if (fromLeftBlue < 3.0)
            {
                leftBlue = true;
            }
        }

        private void clock_Tick(object sender, System.EventArgs e)
        {
            if (left)
            {
                if (SpeedUp > tp)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    clock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    clock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            else
            {
                if (SpeedUp > 0 && tp > SpeedUp)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    clock.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    clock.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            if (speedCount > 40) { SpeedUp = 0.0; }

            fromTop = Canvas.GetTop(fishImage);
            tp = Canvas.GetLeft(fishImage);

            //double wl = Canvas.ActualHeightProperty(fishImage);
            if (left)
            {
                tp += 3;
                //if (up) { mrg += 2; }
                //else { mrg -= 2; }

                //if (mrg > 115 && up) { up = false; }
                //if (mrg < 75 && !up) { up = true; }           
                //fishImage.Margin = new Thickness(0, mrg, 0, 0);
            }
            else tp -= 3;

            Canvas.SetTop(fishImage, fromTop);
            Canvas.SetLeft(fishImage, tp);

            double widthCanvas = cnvFishTank.ActualWidth - fishImage.ActualWidth;
            if (tp >= widthCanvas)
            {
                left = false;
            }
            else if (tp < 3.0)
            {
                left = true;
            }
        }

        private void bubbleClock_Tick(object sender, EventArgs e)
        {
            KeyValuePair<string, System.Windows.Point> kp = (KeyValuePair<string, System.Windows.Point>)bubbleClock.Tag;
            System.Windows.Point pnt = kp.Value;
        
            switch (kp.Key)
            {
                case "fishImage":
                    if (left) { pnt.X = pnt.X + 55; }
                    else { pnt.X = pnt.X + 50; }
                    pnt.Y = pnt.Y + 20;
                    bubbleGo(pnt);
                    break;
                case "blueFishImage":
                    if (leftBlue) { pnt.X = pnt.X + 60; }
                    else { pnt.X = pnt.X + 50; }
                    pnt.Y = pnt.Y;
                    bubbleGo(pnt);
                    break;
                case "redFishImage":
                    if (leftRed) { pnt.X = pnt.X + 65; }
                    else { pnt.X = pnt.X + 50; }
                    pnt.Y = pnt.Y + 20;
                    bubbleGo(pnt);
                    break;
	            default:
	                break;
            }            
        }

        private void bubbleGo(System.Windows.Point pnt)
        {
            if (bubbleCount == 0)
            {
                Canvas.SetLeft(imgBubble1, pnt.X);
                Canvas.SetTop(imgBubble1, pnt.Y);
                imgBubble1.Visibility = Visibility.Visible;
                bub1Top = Canvas.GetTop(imgBubble1);
                bub1Top -= 10;
            }
            else if (bubbleCount == 1)
            {
                Canvas.SetTop(imgBubble1, bub1Top);
                bub1Top -= 10;
            }
            else if (bubbleCount == 2)
            {
                Canvas.SetLeft(imgBubble2, pnt.X);
                Canvas.SetTop(imgBubble2, pnt.Y);
                imgBubble2.Visibility = Visibility.Visible;
                bub2Top = Canvas.GetTop(imgBubble2);
                bub2Top -= 10;
                Canvas.SetTop(imgBubble1, bub1Top);
                bub1Top -= 10;
            }
            else if (bubbleCount == 3)
            {
                Canvas.SetTop(imgBubble1, bub1Top);
                bub1Top -= 10;
                Canvas.SetTop(imgBubble2, bub2Top);
                bub2Top -= 10;
            }
            else if (bubbleCount == 4)
            {

                Canvas.SetLeft(imgBubble3, pnt.X);
                Canvas.SetTop(imgBubble3, pnt.Y);
                imgBubble3.Visibility = Visibility.Visible;
                bub3Top = Canvas.GetTop(imgBubble3);
                bub3Top -= 8;
                Canvas.SetTop(imgBubble1, bub1Top);
                bub1Top -= 10;
                Canvas.SetTop(imgBubble2, bub2Top);
                bub2Top -= 10;
            }
            else
            {
                Canvas.SetTop(imgBubble1, bub1Top);
                bub1Top -= 10;
                Canvas.SetTop(imgBubble2, bub2Top);
                bub2Top -= 10;
                Canvas.SetTop(imgBubble3, bub3Top);
                bub3Top -= 8;
            }
            bubbleCount++;
        }

        private void PrepPics()
        {
            Canvas.SetLeft(fishImage, tp);
            Canvas.SetTop(fishImage, fromTop);

            Canvas.SetLeft(blueFishImage, fromLeftBlue);
            Canvas.SetTop(blueFishImage, fromTopBlue);

            Canvas.SetLeft(redFishImage, tpRed);
            Canvas.SetTop(redFishImage, fromTopRed);

            Canvas.SetTop(imgBowl, cnvFishTank.ActualHeight - imgBowl.ActualHeight);
            Canvas.SetLeft(imgBowl, cnvFishTank.ActualWidth - imgBowl.ActualWidth);
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            ShowImage();
            da.BeginAnimation(System.Windows.Controls.Image.WidthProperty, da);
        }

        private void ShowImage()
        {
            if (left)
            {
                fishImage.Source = rightPics[ctr];
            }
            else
            {
                fishImage.Source = leftPics[ctr];
            }

            if (leftRed)
            {
                redFishImage.Source = RedRightPics[ctr];
            }
            else
            {
                redFishImage.Source = RedLeftPics[ctr];
            }

            if (leftBlue)
            {
                blueFishImage.Source = blueRightPics[ctr];
            }
            else
            {
                blueFishImage.Source = blueLeftPics[ctr];
            }

            ctr++;
            if (ctr >= 20)
            {
                ctr = 0;
            }
        }

        public List<BitmapImage> GetPics(string picFile, int framecount)
        {
            Bitmap b = (Bitmap)System.Drawing.Image.FromFile(@picFile, true);

            if (!Bitmap.IsCanonicalPixelFormat(b.PixelFormat) || !Bitmap.IsAlphaPixelFormat(b.PixelFormat))
                throw new ApplicationException("The picture must be 32bit picture with alpha channel.");
            framecount = 20;
            int FrameWidth = b.Width / framecount;
            int FrameHeight = b.Height;
            List<BitmapImage> picss = new List<BitmapImage>();

            for (int i = 0; i < framecount; i++)
            {
                Bitmap bitmap = new Bitmap(FrameWidth, FrameHeight);
                using (Graphics g = Graphics.FromImage(bitmap))
                    g.DrawImage(b, new System.Drawing.Rectangle(0, 0, FrameWidth, FrameHeight), new System.Drawing.Rectangle(FrameWidth * i, 0, FrameWidth, FrameHeight), GraphicsUnit.Pixel);

                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    picss.Add(bitmapImage);
                }

            }
            return picss;
        }

        private void fishImage_MouseEnter(object sender, MouseEventArgs e)
        {
            speedCount = 0;
            var image = e.Source as System.Windows.Controls.Image;
            System.Windows.Point pnt = new System.Windows.Point { X = Canvas.GetLeft(image), Y = Canvas.GetTop(image) };
            if (left) SpeedUp = tp + 36;
            else { SpeedUp = tp - 36; }
            var position = new KeyValuePair<string, System.Windows.Point>();
            position =(new KeyValuePair<string, System.Windows.Point>( image.Name, pnt));
            bubbleClock.Tag = position;
            bubbleCount = 0;
            bubbleClock.Start();
        }

        private void fishImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as System.Windows.Controls.Image;
                               
            if (image != null && cnvFishTank.CaptureMouse())
            {
                switch (image.Name)
                {
                    case "fishImage":
                        clock.Stop();
                        break;
                    case "blueFishImage":
                        blueClock.Stop();
                        break;
                    case "redFishImage":
                        redClock.Stop();
                        break;
                    default:
                        break;
                }
                bubbleClock.Stop();

                mousePosition = e.GetPosition(cnvFishTank);
                caughtFish = image;
                Panel.SetZIndex(caughtFish, 1);
                //DragDrop.DoDragDrop(image, image, DragDropEffects.None);
            }
        }

        private void fishImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (caughtFish != null)
            {
                cnvFishTank.ReleaseMouseCapture();
                var position = e.GetPosition(cnvFishTank);
                var offset = position - mousePosition;

                Canvas.SetLeft(caughtFish, Canvas.GetLeft(caughtFish) + offset.X);
                Canvas.SetTop(caughtFish, Canvas.GetTop(caughtFish) + offset.Y);
                Panel.SetZIndex(caughtFish, 1);
        
                double fishTot = (Canvas.GetLeft(fishImage)) + (Canvas.GetTop(fishImage));
                double BowlTot = (Canvas.GetLeft(imgBowl)) + (Canvas.GetTop(imgBowl));
                
                if (fishTot < BowlTot)
                {
                    switch (caughtFish.Name)
                    {
                        case "fishImage":
                            clock.Start();
                            break;
                        case "blueFishImage":
                            blueClock.Start();
                            break;
                        case "redFishImage":
                            redClock.Start();
                            break;
                        default:
                            break;
                    }             
                    bubbleCount = 0;
                    bubbleClock.Start();
                }
                caughtFish = null;
            }
        }

        private void fishImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && caughtFish != null)
            {
                var position = e.GetPosition(cnvFishTank);
                var offset = position - mousePosition;
                mousePosition = position;

                Canvas.SetLeft(caughtFish, Canvas.GetLeft(caughtFish) + offset.X);
                Canvas.SetTop(caughtFish, Canvas.GetTop(caughtFish) + offset.Y);
            }
        }

        #endregion
    }
}
