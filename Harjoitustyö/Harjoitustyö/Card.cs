using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Card
    {
        private string Suit;
        private string Value;

        public Card(string values, string suits)
        {
            Value = values;
            Suit = suits;
            
        }

        public override string ToString()
        {
            return Value + " of " + Suit;
        }

        public string getSuit()
        {
            return Suit;
        }

        public string getValue()
        {
            return Value;
        }
    }
}