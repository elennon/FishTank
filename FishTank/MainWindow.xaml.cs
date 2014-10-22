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
using WPF.MDI;

namespace FishTank
{
    
    public partial class MainWindow : Window
    {
       
        public MainWindow()
		{
			InitializeComponent();
			Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();           
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mdiFishTank.Visibility = System.Windows.Visibility.Hidden;
        }

		#region Menu Events

		private void AddWindow_Click(object sender, RoutedEventArgs e)
		{
            Container.Children.Add(new MdiChild
            {
                Title = "Level One",
                Content = new FishLevelOne(),
                Width = 725,
                Height = 550
               // Position = new System.Windows.Point(200, 30)
            });
		}

		void Menu_RefreshWindows()
		{
			WindowsMenu.Items.Clear();
			MenuItem mi;
			for (int i = 0; i < Container.Children.Count; i++)
			{
				MdiChild child = Container.Children[i];
				mi = new MenuItem { Header = child.Title };
				mi.Click += (o, e) => child.Focus();
				WindowsMenu.Items.Add(mi);
			}
			WindowsMenu.Items.Add(new Separator());
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Cascade" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.Cascade;
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Horizontally" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileHorizontal;
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Vertically" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileVertical;

			WindowsMenu.Items.Add(new Separator());
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Close all" });
			mi.Click += (o, e) => Container.Children.Clear();
		}

		#endregion

       

        #region Theme Menu Events

        private void Generic_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = true;
            Luna.IsChecked = false;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Generic;
        }

        private void Luna_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = true;
            Aero.IsChecked = false;

            Container.Theme = ThemeType.Luna;
        }

        private void Aero_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = false;
            Aero.IsChecked = true;

            Container.Theme = ThemeType.Aero;
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
