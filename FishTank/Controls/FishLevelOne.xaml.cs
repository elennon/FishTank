﻿using System;
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
        private DispatcherTimer counterClock = new DispatcherTimer();

        public bool up = true, OKDrop = false, left = true, leftBlue = false, leftRed = true, isOpedFromSavedGame = false;
        public double tp = 0.0, fromTop = 45, SpeedUp = 0.0;
        public double fromLeftBlue = 0.0, fromTopBlue = 110;
        public double tpRed = 200.0, fromTopRed = 230;
        public double bub1Top = 0.0, bub2Top = 0.0, bub3Top = 0.0;
        public int speedCount = 0, bubbleCount = 0, ctr = 0, timerCount = 10, caughtFishCount = 0;
        private System.Windows.Controls.Image caughtFish;
        private System.Windows.Point mousePosition;

        #endregion

        #region DependencyProperty variables

        public static readonly DependencyProperty highestScore =
        DependencyProperty.Register("Score", typeof(string), typeof(FishLevelOne), new PropertyMetadata("0"));

        public string Score
        {
            get { return (string)GetValue(highestScore); }
            set { SetValue(highestScore, value); }
        }

        public static readonly DependencyProperty _greenFish =
        DependencyProperty.Register("greenFish", typeof(Fish), typeof(FishLevelOne), new PropertyMetadata(new Fish()));

        public Fish greenFish
        {
            get { return (Fish)GetValue(_greenFish); }
            set { SetValue(_greenFish, value); }
        }

        public static readonly DependencyProperty _blueFish =
        DependencyProperty.Register("blueFish", typeof(Fish), typeof(FishLevelOne), new PropertyMetadata(new Fish()));

        public Fish blueFish
        {
            get { return (Fish)GetValue(_blueFish); }
            set { SetValue(_blueFish, value); }
        }

        public static readonly DependencyProperty _redFish =
        DependencyProperty.Register("redFish", typeof(Fish), typeof(FishLevelOne), new PropertyMetadata(new Fish()));

        public Fish redFish
        {
            get { return (Fish)GetValue(_redFish); }
            set { SetValue(_redFish, value); }
        }

        #endregion


        public FishLevelOne()
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
            Canvas.SetLeft(imgCountDown, cnvFishTank.ActualWidth * 1.2);

            Canvas.SetTop(imgLose, cnvFishTank.ActualHeight / 3.5);
            Canvas.SetLeft(imgLose, cnvFishTank.ActualWidth * 1.1);
 
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
            

            bubbleClock.Interval = new System.TimeSpan(0, 0, 0, 0, 50);
            bubbleClock.Tick += bubbleClock_Tick;

            counterClock.Interval = new System.TimeSpan(0, 0, 0, 1);
            counterClock.Tick += counterClock_Tick;

            if (isOpedFromSavedGame) 
            { 
                setFisToOriginalPositions(); 
            }
            else { setFish(); }
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
                fromLeftBlue += 3;
            }
            else fromLeftBlue -= 3;

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
                tp += 3;
                //if (up) { mrg += 2; }
                //else { mrg -= 2; }

                //if (mrg > 115 && up) { up = false; }
                //if (mrg < 75 && !up) { up = true; }           
                //fishImage.Margin = new Thickness(0, mrg, 0, 0);
            }
            else tp -= 3;

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

        public void setFisToOriginalPositions()
        {
            double BowlTop = 375;// Canvas.GetLeft(imgBowl);
            double bowlLeft = 561;// Canvas.GetTop(imgBowl);

            if(greenFish.Caught)
            {             
                Canvas.SetTop(greenFishImage, BowlTop + 5);
                Canvas.SetLeft(greenFishImage, bowlLeft + 5);
                Panel.SetZIndex(greenFishImage, 1);
            }
            else
            {
                Canvas.SetTop(greenFishImage, fromTop);
                clock.Start();
            }

            if (blueFish.Caught)
            {
                
                Canvas.SetTop(blueFishImage, BowlTop + 5);
                Canvas.SetLeft(blueFishImage, bowlLeft + 5);
                Panel.SetZIndex(blueFishImage, 1);
            }
            else
            {
                Canvas.SetTop(blueFishImage, fromTopBlue);
                Canvas.SetLeft(blueFishImage, fromLeftBlue);
                blueClock.Start();
            }

            if (redFish.Caught)
            {
                Canvas.SetTop(redFishImage, BowlTop);
                Canvas.SetLeft(redFishImage, bowlLeft);
                Panel.SetZIndex(redFishImage, 1);
            }
            else
            {
                Canvas.SetTop(redFishImage, fromTopRed);
                Canvas.SetLeft(redFishImage, tpRed);
                redClock.Start();
            }           
        }

        public void setFish()
        {
            greenFish.Colour = "green";
            greenFish.Name = "greenFish";
            greenFish.Caught = false;
            blueFish.Colour = "blue";
            blueFish.Name = "blueFish";
            blueFish.Caught = false;
            redFish.Colour = "red";
            redFish.Name = "redFish";
            redFish.Caught = false;
            
            Canvas.SetTop(greenFishImage, fromTop);
            Canvas.SetTop(blueFishImage, fromTopBlue);
            Canvas.SetLeft(blueFishImage, fromLeftBlue);
            Canvas.SetTop(redFishImage, fromTopRed);
            Canvas.SetLeft(redFishImage, tpRed);
            clock.Start();
            redClock.Start();
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
                greenFishImage.Source = rightPics[ctr];
            }
            else
            {
                greenFishImage.Source = leftPics[ctr];
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
                //DragDrop.DoDragDrop(image, image, DragDropEffects.None);
            }
        }

        private void fishImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (caughtFish != null)
            {
                caughtFishCount++;

                setToCaught(caughtFish.Name);

                cnvFishTank.ReleaseMouseCapture();
                var position = e.GetPosition(cnvFishTank);
                var offset = position - mousePosition;

                double dd = Canvas.GetLeft(caughtFish) + offset.X;
                double ty = Canvas.GetTop(caughtFish) + offset.Y;

                Canvas.SetLeft(caughtFish, Canvas.GetLeft(caughtFish) + offset.X);
                Canvas.SetTop(caughtFish, Canvas.GetTop(caughtFish) + offset.Y);
                Panel.SetZIndex(caughtFish, 1);

                double fishTot = (Canvas.GetLeft(caughtFish)) + (Canvas.GetTop(caughtFish));
                double BowlTot = (Canvas.GetLeft(imgBowl)) + (Canvas.GetTop(imgBowl));
                
                if (fishTot < BowlTot)
                {                    
                    switch (caughtFish.Name)
                    {
                        case "greenFishImage":   
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
                if(greenFish.Caught &&  blueFish.Caught && redFish.Caught)
                {
                    counterClock.Stop();
                    if(Score != null)
                    {
                        if (Convert.ToInt32(Score) < timerCount + 1)
                        {
                            Score = (timerCount + 1).ToString();
                        }                       
                    }                   
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

        private void setToCaught(string nme)
        {
            switch (nme)
            {
                case "greenFishImage":
                    greenFish.Caught = true;
                    break;
                case "blueFishImage":
                    blueFish.Caught = true;
                    break;
                case "redFishImage":
                    redFish.Caught = true;
                    break;
                default:
                    break;
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
