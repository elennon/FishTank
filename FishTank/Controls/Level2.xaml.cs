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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FishTank.Controls
{

    public partial class Level2 : UserControl
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
        private DispatcherTimer redClock2 = new DispatcherTimer();
        private DispatcherTimer bubbleClock = new DispatcherTimer();
        private DispatcherTimer counterClock = new DispatcherTimer();

        public bool up = true, OKDrop = false, left = true, leftBlue = false, leftRed = true, leftRed2 = false, isOpedFromSavedGame = false;
        public double tp = 0.0, fromTop = 145, SpeedUp = 0.0;
        public double fromLeftBlue = 0.0, fromTopBlue = 210;
        public double tpRed = 200.0, fromTopRed = 330, tpRed2 = 80.0, fromTopRed2 = 80;
        public double bub1Top = 0.0, bub2Top = 0.0, bub3Top = 0.0;
        public int speedCount = 0, bubbleCount = 0, ctr = 0, timerCount = 10, caughtFishCount = 0;
        private System.Windows.Controls.Image caughtFish;
        private System.Windows.Point mousePosition;
        private string score = "0";

        #endregion

      
        public Level2()
        {
            InitializeComponent();

            leftPics = GetPics("GrnLeft.png", 20);
            rightPics = GetPics("GrnRight.png", 20);

            blueLeftPics = GetPics("BlueLeft.png", 20);
            blueRightPics = GetPics("BlueRight.png", 20);

            RedLeftPics = GetPics("RedLeft.png", 20);
            RedRightPics = GetPics("RedRight.png", 20);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/space.jpg"));
            cnvFishTank.Background = imageBrush;
            Canvas.SetTop(imgCountDown, cnvFishTank.ActualHeight / 3.5);
            Canvas.SetLeft(imgCountDown, cnvFishTank.ActualWidth * 1.05);

            Canvas.SetTop(imgLose, cnvFishTank.ActualHeight / 3.5);
            Canvas.SetLeft(imgLose, cnvFishTank.ActualWidth * 0.9);

            PrepPics();
            imgBubble1.Visibility = Visibility.Hidden;
            imgBubble2.Visibility = Visibility.Hidden;
            imgBubble3.Visibility = Visibility.Hidden;
            clock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            clock.Tick += clock_Tick;


            blueClock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            blueClock.Tick += blueClock_Tick;
            fromLeftBlue = cnvFishTank.ActualWidth - 55;

            redClock.Interval = new System.TimeSpan(0, 0, 0, 0, 36);
            redClock.Tick += redClock_Tick;

            redClock2.Interval = new System.TimeSpan(0, 0, 0, 0, 36);
            redClock2.Tick += redClock2_Tick;

            bubbleClock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            bubbleClock.Tick += bubbleClock_Tick;

            counterClock.Interval = new System.TimeSpan(0, 0, 0, 1);
            counterClock.Tick += counterClock_Tick;
            setFish();
            
        }

        #region Clock tick Events

        void counterClock_Tick(object sender, EventArgs e)
        {
            if (timerCount >= 0)
            {
                string countPic = "pack://application:,,,/Images/no" + timerCount.ToString() + ".png";
                imgCountDown.Source = new BitmapImage(new Uri(countPic));
                timerCount--;
            }
            else
            {
                imgLose.Visibility = System.Windows.Visibility.Visible;
                redClock.Stop();
                blueClock.Stop();
                clock.Stop();
                counterClock.Stop();
            }
        }

        void redClock2_Tick(object sender, EventArgs e)
        {
            if (leftRed2)
            {
                if (SpeedUp > tpRed2)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    redClock2.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    redClock2.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            else
            {
                if (SpeedUp > 0 && tpRed2 > SpeedUp)
                {
                    speedCount++;
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.01));
                    da.Duration = duration;
                    redClock2.Interval = new System.TimeSpan(0, 0, 0, 0, 1);
                }
                else
                {
                    Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                    da.Duration = duration;
                    redClock2.Interval = new System.TimeSpan(0, 0, 0, 0, 40);
                }
            }
            if (speedCount > 40) { SpeedUp = 0.0; }

            fromTopRed2 = Canvas.GetTop(redFishImage2);
            tpRed2 = Canvas.GetLeft(redFishImage2);

            //double wl = Canvas.ActualHeightProperty(fishImage);
            if (leftRed2)
            {
                tpRed2 += 5;
            }
            else tpRed2 -= 5;

            Canvas.SetTop(redFishImage2, fromTopRed2);
            Canvas.SetLeft(redFishImage2, tpRed2);

            double widthCanvas = cnvFishTank.ActualWidth - greenFishImage.ActualWidth;
            if (tpRed2 >= widthCanvas)
            {
                leftRed2 = false;
            }
            else if (tpRed2 < 3.0)
            {
                leftRed2 = true;
            }
        }

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
                tpRed += 5;
            }
            else tpRed -= 5;

            Canvas.SetTop(redFishImage, fromTopRed);
            Canvas.SetLeft(redFishImage, tpRed);

            double widthCanvas = cnvFishTank.ActualWidth - greenFishImage.ActualWidth;
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
                fromLeftBlue += 5;
            }
            else fromLeftBlue -= 5;

            Canvas.SetTop(blueFishImage, fromTopBlue);
            Canvas.SetLeft(blueFishImage, fromLeftBlue);

            double widthCanvas = cnvFishTank.ActualWidth - greenFishImage.ActualWidth;
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

            fromTop = Canvas.GetTop(greenFishImage);
            tp = Canvas.GetLeft(greenFishImage);

            //double wl = Canvas.ActualHeightProperty(fishImage);
            if (left)
            {
                tp += 5;
                //if (up) { mrg += 2; }
                //else { mrg -= 2; }

                //if (mrg > 115 && up) { up = false; }
                //if (mrg < 75 && !up) { up = true; }           
                //fishImage.Margin = new Thickness(0, mrg, 0, 0);
            }
            else tp -= 5;

            Canvas.SetTop(greenFishImage, fromTop);
            Canvas.SetLeft(greenFishImage, tp);

            double widthCanvas = cnvFishTank.ActualWidth - greenFishImage.ActualWidth;
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
                case "greenFishImage":
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

        #endregion

        #region setting up

        public void setFish()
        {
            if (cnvFishTank.Children.Contains(greenFishImage) == false) { cnvFishTank.Children.Add(greenFishImage); }
            if (cnvFishTank.Children.Contains(blueFishImage) == false) { cnvFishTank.Children.Add(blueFishImage); }
            if (cnvFishTank.Children.Contains(redFishImage) == false) { cnvFishTank.Children.Add(redFishImage); }
            if (cnvFishTank.Children.Contains(redFishImage2) == false) { cnvFishTank.Children.Add(redFishImage2); }
            
            Canvas.SetTop(greenFishImage, fromTop);
            Canvas.SetTop(blueFishImage, fromTopBlue);
            Canvas.SetLeft(blueFishImage, fromLeftBlue);
            Canvas.SetTop(redFishImage, fromTopRed);
            Canvas.SetLeft(redFishImage, tpRed);
            Canvas.SetTop(redFishImage2, fromTopRed2);
            Canvas.SetLeft(redFishImage2, tpRed2);
            clock.Start();
            redClock.Start();
            redClock2.Start();
            blueClock.Start();
        }

        private void PrepPics()
        {
            Canvas.SetLeft(greenFishImage, tp);
            Canvas.SetTop(greenFishImage, fromTop);

            Canvas.SetLeft(blueFishImage, fromLeftBlue);
            Canvas.SetTop(blueFishImage, fromTopBlue);

            Canvas.SetLeft(redFishImage, tpRed);
            Canvas.SetTop(redFishImage, fromTopRed);

            Canvas.SetTop(bowlGrid, cnvFishTank.ActualHeight - imgBowl.ActualHeight);
            Canvas.SetLeft(bowlGrid, (cnvFishTank.ActualWidth / 2) + (imgBowl.ActualWidth / 6) );
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
                greenFishImage.Source = rightPics[ctr];
            }
            else
            {
                greenFishImage.Source = leftPics[ctr];
            }

            if (leftRed2)
            {
                redFishImage2.Source = RedRightPics[ctr];
            }
            else
            {
                redFishImage2.Source = RedLeftPics[ctr];
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
            DirectoryInfo dir = Directory.GetParent(Environment.CurrentDirectory);
            string p = dir.FullName;
            dir = Directory.GetParent(p);
            p = string.Format(@"{0}\Images\{1}", dir, picFile);
            Bitmap b = (Bitmap)System.Drawing.Image.FromFile(p, true);

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

        #endregion

        #region Mouse Events

        private void fishImage_MouseEnter(object sender, MouseEventArgs e)
        {
            speedCount = 0;
            var image = e.Source as System.Windows.Controls.Image;
            System.Windows.Point pnt = new System.Windows.Point { X = Canvas.GetLeft(image), Y = Canvas.GetTop(image) };
            if (left) SpeedUp = tp + 36;
            else { SpeedUp = tp - 36; }
            var position = new KeyValuePair<string, System.Windows.Point>();
            position = (new KeyValuePair<string, System.Windows.Point>(image.Name, pnt));
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
                    case "greenFishImage":
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
                DataObject dragData = new DataObject("Format", image);
                DragDrop.DoDragDrop(image, dragData, DragDropEffects.Move);             
            }
        }

        private void bowlBorder_Drop(object sender, DragEventArgs e)
        {
            System.Windows.Controls.Image pic = e.Data.GetData("Format") as System.Windows.Controls.Image;
            cnvFishTank.Children.Remove(pic);
            caughtFishCount++;
            bowlGrid.Children.Add(pic);
            if(caughtFishCount == 4)
            {
                counterClock.Stop();
                if(score != null)
                {
                    if (Convert.ToInt32(score) < timerCount + 1)
                    {
                        score = (timerCount + 1).ToString();
                        tbkScoree.Text = score;
                    }
                    else { tbkScoree.Text = score; }  
                }                   
            }
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("Format") )
            {
                e.Effects = DragDropEffects.None;
            }
        }

        #endregion

        private void btnTimerGo_Click(object sender, RoutedEventArgs e)
        {
            imgLose.Visibility = Visibility.Hidden;
            setFish();
            timerCount = 10;
            counterClock.Start();
        }        
    }
}
