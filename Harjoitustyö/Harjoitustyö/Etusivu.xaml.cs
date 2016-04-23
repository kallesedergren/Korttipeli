using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Harjoitustyö
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Etusivu : Page
    {
        private Players players = new Players();

        public Etusivu()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // Ikkunan koko
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
        }

        private void updateList()
        {
            pelaajatList.ItemsSource = null;
            List<TextBlock> blocks = new List<TextBlock>();
            foreach(Player p in players.PlayersList)
            {
                TextBlock b = new TextBlock();
                b.Text = p.Name;
                b.FontSize = 20;
                b.Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
                blocks.Add(b);
            }

            pelaajatList.ItemsSource = blocks;
        }

        private void aloitusButton_Click(object sender, RoutedEventArgs e)
        {
            if(players.PlayersList.Count >= 2)
            {
                this.Frame.Navigate(typeof(MainPage), players);
            }
        }

        private void lisaaButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nimiBox.Text;

            if (name != "")
            {
                players.AddPlayer(new Player(name));
                nimiBox.Text = "";
                updateList();
            }
        }

        private void tyhjennaButton_Click(object sender, RoutedEventArgs e)
        {
            players.resetList();
            updateList();
        }
    }
}
