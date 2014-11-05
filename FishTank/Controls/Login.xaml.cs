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
using WPF.MDI;

namespace FishTank.Controls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            txtPlayerName.Focus();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Global.player = txtPlayerName.Text;
            MainWindow CtrlWindow = new MainWindow();
            Global.close = true;

            CtrlWindow.ShowDialog();
        }
    }
}
