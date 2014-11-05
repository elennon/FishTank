using FishTank.Controls;
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
using System.Xml.Serialization;
using WPF.MDI;

namespace FishTank
{
    
    public partial class MainWindow : Window
    {
        public string player = "";

        public MainWindow()
		{
			InitializeComponent();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Player> players = getPlayers();
            foreach (var item in players)
            {
                MenuItem mi = new MenuItem { Header = item.PlayerName, Tag= item.PlayerName };
                mi.Click += Open_Click;
                miOpen.Items.Add(mi);
            }
            

            mdiFishTank.Visibility = System.Windows.Visibility.Hidden;
            if (Global.close == false)
            {
                Container.Children.Add(new MdiChild
                {
                    Title = "Player Login",
                    Content = new Login(),
                    Width = 514,
                    Height = 534,
                    Name = "nmeLogin"
                });
            }
            else
            {
                player = Global.player;
                PlayerName.Header = "Player:    " + Global.player;               
            }
        }

        private List<Player> getPlayers()
        {
            List<Player> players = new List<Player>(); 
            DirectoryInfo dir = Directory.GetParent(Environment.CurrentDirectory);
            string file = dir.FullName;
            dir = Directory.GetParent(file);
            file = string.Format(@"{0}\SavedGames.xml", dir);

            XmlSerializer xs = new XmlSerializer(typeof(Games), new Type[] { typeof(Player), typeof(Fish) });

            using (Stream str = File.OpenRead(file))
            {
                Games savedGame = (Games)xs.Deserialize(str);
                if(savedGame != null)
                {
                    foreach (Player item in savedGame.players)
                    {
                        players.Add(item);
                    }
                }
            }
            return players;
        }

		#region Game Menu Events
        private void AddRules_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild
            {
                Title = "Game Rules",
                Content = new Rules(),
                Width = 600,
                Height = 550, Name = "LoginPg"
                // Position = new System.Windows.Point(200, 30)
            });
        }

		private void AddWindow_Click(object sender, RoutedEventArgs e)
		{
            player = Global.player;
            Container.Children.Add(new MdiChild
            {
                Title = "Level One",
                Content = new FishLevelOne(),
                Width = 725,
                Height = 550,
                Name = "levelOne"
               // Position = new System.Windows.Point(200, 30)
            });
		}

        private void AddLevel2_Click(object sender, RoutedEventArgs e)
        {
            player = Global.player;
            Container.Children.Add(new MdiChild
            {
                Title = "Level One",
                Content = new Level2(),
                Width = 925,
                Height = 650,
                Name = "levelTwo"
                // Position = new System.Windows.Point(200, 30)
            });
        }

		
		#endregion

       
        #region Save and Open Menu Events

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = Directory.GetParent(Environment.CurrentDirectory);
            string file = dir.FullName;
            dir = Directory.GetParent(file);
            file = string.Format(@"{0}\SavedGames.xml", dir);

            XmlSerializer xs = new XmlSerializer(typeof(Games), new Type[] { typeof(Player), typeof(Fish) });
            Games savedGames = new Games();
            using (Stream str = File.OpenRead(file))
            {
                savedGames = (Games)xs.Deserialize(str);
                if (savedGames == null) { savedGames = new Games(); }   // incase xml fil is empty

                var pl = savedGames.players.Where(a => a.PlayerName == player).FirstOrDefault();    // if player already there, clear for new record
                if (pl != null) { savedGames.players.Remove(pl); }

                Player playr = new Player { PlayerName = player, HighestScore = LevelOne.Score };
                playr.fishes = new List<Fish> { LevelOne.greenFish, LevelOne.blueFish, LevelOne.redFish };
                savedGames.players.Add(playr);
            }

            using (Stream str = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xs.Serialize(str, savedGames);
            }
            MessageBox.Show("Game Saved");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            MenuItem playerToOpen = (MenuItem)sender;
            string platerName = playerToOpen.Tag.ToString();
            Player playrr = new Player();

            DirectoryInfo dir = Directory.GetParent(Environment.CurrentDirectory);
            string file = dir.FullName;
            dir = Directory.GetParent(file);
            file = string.Format(@"{0}\SavedGames.xml", dir);

            XmlSerializer xs = new XmlSerializer(typeof(Games), new Type[] { typeof(Player), typeof(Fish) });

            using (Stream str = File.OpenRead(file))
            {
                Games savedGame = (Games)xs.Deserialize(str);
                playrr = savedGame.players.Where(a => a.PlayerName == platerName).FirstOrDefault();
            }

            FishLevelOne fl1 = new FishLevelOne();
            fl1.isOpedFromSavedGame = true;
            foreach (var item in playrr.fishes)
            {
                switch (item.Colour)
                {
                    case "green":
                        fl1.greenFish = item;
                        break;
                    case "blue":
                        fl1.blueFish = item;
                        break;
                    case "red":
                        fl1.redFish = item;
                        break;                    
                }             
            }
            Container.Children.Add(new MdiChild
            {
                Title = "Level One",
                Content = fl1,
                Width = 725,
                Height = 550,
                Name = "levelOne"               
            });
        }

        #endregion

		#region Content Button Events

		private void DisableMinimize_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.MinimizeBox = false;
		}

		private void EnableMinimize_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.MinimizeBox = true;
		}

		private void DisableMaximize_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.MaximizeBox = false;
		}

		private void EnableMaximize_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.MaximizeBox = true;
		}

		private void ShowIcon_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.ShowIcon = true;
		}

		private void HideIcon_Click(object sender, RoutedEventArgs e)
		{
            mdiFishTank.ShowIcon = false;
		}
		#endregion

        
	}

    
}
