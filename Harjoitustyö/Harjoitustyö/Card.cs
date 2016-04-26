using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Card
    {

        // luokan muuttujat
        private string Suit;
        private string Value;

        // konstruktori
        public Card(string values, string suits)
        {
            Value = values;
            Suit = suits;
        }

        // luokan metodi joka palauttaa kortin maan
        public string getSuit()
        {
            return Suit;
        }
        // luokan metodi joka palauttaa kortin arvon
        public string getValue()
        {
            return Value;
        }
    }
}