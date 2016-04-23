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
        Deck deck = new Deck();
        Players players;
        int vuoro = 0;
        private bool mediaPlaying = false;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // Ikkunan koko
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            deck.Shuffle();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            players = (Players)e.Parameter;
            vuorossa.Text = "Vuorossa: " + players.PlayersList.ElementAt(vuoro).Name;
            updateList();
        }

        public void image_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (mediaPlaying) return;

            if (players.PlayersList.ElementAt(vuoro).tauolla)
            {
                players.PlayersList.ElementAt(vuoro).tauolla = false;

            }

            Card card = deck.dealCard();
            try
            {
                string filePath = @"Assets/Images/" + card.getValue() + "_of_" + card.getSuit() + ".png";
                string rulePath = @"Assets/Rules/" + card.getValue() + ".txt";
                string readRules = File.ReadAllText(rulePath);

                cardsUp.Source = new BitmapImage(new Uri(this.BaseUri, filePath));
                rule.Text = readRules;

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
                        players.PlayersList.ElementAt(vuoro).kysymysMestari = true;
                        break;

                    case "7":
                        players.PlayersList.ElementAt(vuoro).addTauko();
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

        private void updateHuoraList()
        {
            huoraList.ItemsSource = null;

            Player vuorossa = players.PlayersList.ElementAt(vuoro);

            List<string> huorat = new List<string>();

            foreach (Player p in players.PlayersList)
            {
                if(p != vuorossa && !vuorossa.huorat.Contains(p))
                {
                    huorat.Add(p.Name);
                }
            }

            if (!huorat.Any())
            {
                annaHuora.Content = "SULJE";
            }
            else
            {
                annaHuora.Content = "ANNA";
            }

            huoraList.ItemsSource = huorat;
        }

        private void checkBreak()
        {
            Player p = players.PlayersList.ElementAt(vuoro);
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

        private void nextTurn()
        {
            if (deck.isEmpty()) vuoro++;
            if (vuoro >= players.PlayersList.Count) vuoro = 0;
            updateList();
            vuorossa.Text = "Vuorossa: " + players.PlayersList.ElementAt(vuoro).Name;
            checkBreak();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            deck.Shuffle();
            rule.Text = "";
            cardsUp.Source = null;
            players.resetStatus();
            updateList();
        }

        private void kaytatauko_Tapped(object sender, TappedRoutedEventArgs e)
        {
            onkotauko.Visibility = Visibility.Collapsed;
        }

        private void updateList()
        {
            pelaajatList.ItemsSource = null;

            List<TextBlock> blocks = new List<TextBlock>();
            foreach (Player p in players.PlayersList)
            {
                TextBlock b = new TextBlock();
                b.Text = p.ToString();
                b.Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
                b.FontSize = 20;
                blocks.Add(b);
            }

            pelaajatList.ItemsSource = blocks;
            pelaajatList.SelectedIndex = vuoro;
        }

        private void breakButton_Click(object sender, RoutedEventArgs e)
        {
            players.PlayersList.ElementAt(vuoro).substractTauko();
            checkBreak();
            nextTurn();
        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            nextTurn();
        }

        private void huoraList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void annaHuora_Click(object sender, RoutedEventArgs e)
        {

            if (huoraList.Items.Any())
            {
                if (huoraList.SelectedItem == null) return;
                Player p = players.getByName(huoraList.SelectedItem.ToString());
                players.PlayersList.ElementAt(vuoro).huorat.Add(p);
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