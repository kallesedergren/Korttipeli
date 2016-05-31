using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Card
    {
        // Luokan muuttujat
        private string Suit;
        private string Value;

        // Konstruktori
        public Card(string values, string suits)
        {
            Value = values;
            Suit = suits;
        }

        // Luokan metodi joka palauttaa kortin maan
        public string getSuit()
        {
            return Suit;
        }

        // Luokan metodi joka palauttaa kortin arvon
        public string getValue()
        {
            return Value;
        }
    }
}