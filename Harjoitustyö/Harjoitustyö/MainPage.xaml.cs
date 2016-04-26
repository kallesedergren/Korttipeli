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
using Windows.UI.ViewManagement;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Harjoitustyö
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        // alustetaan pakka
        Deck deck = new Deck();
        // alustetaan pelaajalista
        Players players;
        // muuttuja, joka kertoo kenen vuoro on
        int turn = 0;
        // soiko musiikki
        private bool mediaPlaying = false;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // Ikkunan koko
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            // sekoitetaan pakka
            deck.Shuffle();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // pelisivulle siirryttäessä haetaan pelaajalista
            base.OnNavigatedTo(e);
            players = (Players)e.Parameter;
            vuorossa.Text = "Vuorossa: " + players.PlayersList.ElementAt(turn).Name;
            updateList();
        }

        public void image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // jos media soi, ei tehdä mitään
            if (mediaPlaying) return;

            // jos tauolla oleva pelaaja painaa korttia, hän palaa peliin
            if (players.PlayersList.ElementAt(turn).tauolla)
            {
                players.PlayersList.ElementAt(turn).tauolla = false;
            }

            // nostetaan kortti
            Card card = deck.dealCard();

            try
            {
                // haetaan korttien kuvat ja säännöt
                string filePath = @"Assets/Images/" + card.getValue() + "_of_" + card.getSuit() + ".png";
                string rulePath = @"Assets/Rules/" + card.getValue() + ".txt";
                string readRules = File.ReadAllText(rulePath);

                cardsUp.Source = new BitmapImage(new Uri(this.BaseUri, filePath));
                rule.Text = readRules;

                // näytetään kuva, jos kortti on 4 tai 5
                if (card.getValue().Equals("4") || card.getValue().Equals("5"))
                {
                    hitler.Visibility = Visibility.Visible;
                    mediaPlaying = true;
                    hitlerStoryboard.Begin();
                    hitlerSound.Play();
                }
                else
                {
                    hitler.Visibility = Visibility.Collapsed;
                }

                switch (card.getValue())
                {
                    case "10":
                        players.resetKysymysmestari();
                        players.PlayersList.ElementAt(turn).kysymysMestari = true;
                        break;

                    case "7":
                        players.PlayersList.ElementAt(turn).addTauko();
                        break;

                    case "queen":
                        updateHuoraList();
                        huoraPanel.Visibility = Visibility.Visible;
                        deckFull.Visibility = Visibility.Collapsed;
                        break;
                }
                if(card.getValue() != "queen") nextTurn();
            }
            catch (Exception)
            {
                cardsUp.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Images/Adolf.jpg"));
                rule.Text = "Pakka on juotu...";
            }
        }

        // kun pelaaja nostaa "huora"kortin, näytetään lista pelaajista, joilla kortti voidaan antaa
        private void updateHuoraList()
        {
            huoraList.ItemsSource = null;

            Player vuorossa = players.PlayersList.ElementAt(turn);

            List<string> huorat = new List<string>();

            foreach (Player p in players.PlayersList)
            {
                if(p != vuorossa && !vuorossa.huorat.Contains(p))
                {
                    huorat.Add(p.Name);
                }
            }

            // jos ei voi antaa "huoraa", "ANNA" napin sijasta "SULJE" nappi
            if (!huorat.Any())
            {
                annaHuora.Content = "SULJE";
            }
            else
            {
                annaHuora.Content = "ANNA";
            }

            // näytetään "huorat" ListViewissä
            huoraList.ItemsSource = huorat;
        }

        // tarkistetaan onko pelaaja tauolla
        private void checkBreak()
        {
            Player p = players.PlayersList.ElementAt(turn);
            if (p.Tauot > 0 && !p.tauolla)
            {
                breakButton.Visibility = Visibility.Visible;
            } else
            {
                breakButton.Visibility = Visibility.Collapsed;
            }
            if (p.tauolla)
            {
                skipButton.Visibility = Visibility.Visible;
            } else
            {
                skipButton.Visibility = Visibility.Collapsed;
            }
        }

        // vuoro vaihtuu, jos pakka ei ole tyhjä
        private void nextTurn()
        {
            if (deck.isEmpty()) turn++;
            if (turn >= players.PlayersList.Count) turn = 0;
            updateList();
            vuorossa.Text = "Vuorossa: " + players.PlayersList.ElementAt(turn).Name;
            checkBreak();
        }

        // aloitetaan uusi kierros
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            deck.Shuffle();
            rule.Text = "";
            cardsUp.Source = null;
            players.resetStatus();
            updateList();
        }
        
        // asetetaan pelaajat ListViewiin
        private void updateList()
        {
            pelaajatList.ItemsSource = null;
            // luodaan jokaiselle pelaajalle oma TextBlock listaan
            List<TextBlock> blocks = new List<TextBlock>();
            foreach (Player p in players.PlayersList)
            {
                TextBlock b = new TextBlock();
                b.Text = p.ToString();
                b.Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
                b.FontSize = 20;
                blocks.Add(b);
            }

            // korostetaan vuorossa oleva pelaaja
            pelaajatList.ItemsSource = blocks;
            pelaajatList.SelectedIndex = turn;
        }

        // vähennetään pelaajalta yksi tauko ja asetetaan pelaaja tauolle
        private void breakButton_Click(object sender, RoutedEventArgs e)
        {
            players.PlayersList.ElementAt(turn).substractTauko();
            checkBreak();
            nextTurn();
        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            nextTurn();
        }
        
        private void annaHuora_Click(object sender, RoutedEventArgs e)
        {
            // tarkistaan onko listassa yhtään pelaajaa jollekka "huoran" voi antaa
            if (huoraList.Items.Any())
            {
                if (huoraList.SelectedItem == null) return;
                Player p = players.getByName(huoraList.SelectedItem.ToString());
                players.PlayersList.ElementAt(turn).huorat.Add(p);
            }
            
            huoraPanel.Visibility = Visibility.Collapsed;
            deckFull.Visibility = Visibility.Visible;

            updateList();
            nextTurn();
        }

        private void closeRules_Click(object sender, RoutedEventArgs e)
        {
            saannotStack.Visibility = Visibility.Collapsed;
        }

        private void rules_Click(object sender, RoutedEventArgs e)
        {
            saannotStack.Visibility = Visibility.Visible;
        }

        private void hitlerSound_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlaying = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Etusivu));
        }
    }
}