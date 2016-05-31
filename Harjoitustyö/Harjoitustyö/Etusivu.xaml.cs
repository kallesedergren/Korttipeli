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
        // Alustetaan pelaajalista
        private Players players = new Players();

        public Etusivu()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // Ikkunan koko
            Size windowSize = new Size(1280, 720);
            ApplicationView.PreferredLaunchViewSize = windowSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(windowSize);
        }

        // Päivitetään pelaajalista sitä mukaa kun pelaajia lisätään
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

        // Jos neljä pelaajaa syötetty peli alkaa aloitus-painikkeesta
        private void aloitusButton_Click(object sender, RoutedEventArgs e)
        {
            if(players.PlayersList.Count >= 4)
            {
                this.Frame.Navigate(typeof(MainPage), players);
            }
        }

        // Lisätään syötetty pelaaja
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

        // Tyhjennetään pelaajalista
        private void tyhjennaButton_Click(object sender, RoutedEventArgs e)
        {
            players.resetList();
            updateList();
        }
    }
}