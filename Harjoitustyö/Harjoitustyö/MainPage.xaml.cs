using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Harjoitustyö
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        Deck deck = new Deck();
        List<Player> pelaajat;
        int vuoro = 0;

        public MainPage()
        {
            this.InitializeComponent();
            deck.Shuffle();

            pelaajat = new List<Player>();

            pelaajat.Add(new Player { Name = "Hannes" });
            pelaajat.Add(new Player { Name = "Marko" });
            pelaajat.Add(new Player { Name = "Jesse" });
            pelaajat.Add(new Player { Name = "Herbert" });

            player1.Text = pelaajat.ElementAt(0).Name;
            player2.Text = pelaajat.ElementAt(1).Name;
            player3.Text = pelaajat.ElementAt(2).Name;
            player4.Text = pelaajat.ElementAt(3).Name;

            vuorossa.Text = "Vuorossa: " + pelaajat.ElementAt(vuoro).Name;
        }

        public void image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Card card = deck.dealCard();
            vuoro++;
            if (vuoro >= pelaajat.Count) vuoro = 0; 
            vuorossa.Text = "Vuorossa: " + pelaajat.ElementAt(vuoro).Name;
            try
            {
                string filePath = @"Assets/Images/" + card.getValue() + "_of_" + card.getSuit() + ".png";
                string rulePath = @"Assets/Rules/" + card.getValue() + ".txt";
                string readRules = File.ReadAllText(rulePath);

                cardsUp.Source = new BitmapImage(new Uri(this.BaseUri, filePath));
                rule.Text = readRules;

            } catch (Exception)
            {
                cardsUp.Source = null; /* new BitmapImage(new Uri(this.BaseUri, "Assets/Images/deck.png")); */
                rule.Text = "Pakka on juotu! ( ͡° ͜ʖ ͡°)";
            }

           if (card.getValue().Equals("Four") || card.getValue().Equals("Five"))
            {
                hitler.Visibility = Visibility.Visible;
            }
            else
            { 
                hitler.Visibility = Visibility.Collapsed;
            }
                    
}

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            deck.Shuffle();
            rule.Text = "";
            cardsUp.Source = null;
        }

        private void kaytatauko_Tapped(object sender, TappedRoutedEventArgs e)
        {
            onkotauko.Visibility = Visibility.Collapsed;
        }
    }
}
