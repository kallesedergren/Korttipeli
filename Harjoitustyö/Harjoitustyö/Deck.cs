using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{   
    public class Deck
    {
        // Luokan muuttujat
        private Card[] deck;
        private int currentCard;
        private const int MaxCards = 52;
        private Random ranNum;
         
        // Pakan luonti
        public Deck()
        {
            string[] values = { "ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king" };
            string[] suits = { "hearts", "clubs", "diamonds", "spades" };

            // Alustetaan lista
            deck = new Card[MaxCards];
            currentCard = 0;

            // Luodaan random-olio
            ranNum = new Random();
            for (int count = 0; count < deck.Length; count++)
                deck[count] = new Card(values[count % 13], suits[count / 13]);
        }

        // Pakan sekoitus
        public void Shuffle()
        {
            currentCard = 0;
            for (int first = 0; first < deck.Length; first++)
            {
                int second = ranNum.Next(MaxCards);
                Card temp = deck[first];
                deck[first] = deck[second];
                deck[second] = temp;
            }
        }

        // Tarkistetaan, onko pakassa vielä kortteja
        public bool isEmpty()
        {
            return deck.Any();
        }

        // Nostetaan pakasta yksi kortti
        public Card dealCard()
        {
            if (currentCard < deck.Length)
            {
                return deck[currentCard++];
            }
            else
            {
                return null;
            }
        }
    }
}