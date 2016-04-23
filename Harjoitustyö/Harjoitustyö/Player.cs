using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Player
    {
        public string Name { get; set; }

        public bool kysymysMestari = false;

        public bool tauolla = false;

        public List<Player> huorat = new List<Player>();

        private int tauot = 0;
        public int Tauot
        {
            get { return tauot; }
            set { tauot = value; }
        }

        public Player(string name)
        {
            Name = name;
        }

        public Player()
        { }

        public void addTauko()
        {
            tauot++;
        }

        public void substractTauko()
        {
            tauolla = true;
            tauot--;
        }

        public override string ToString()
        {
            return Name + getStatus(); ;
        }

        public string getStatus()
        {
            string status = kysymysMestari ? " K" : "";
            status += tauot > 0 ? " T" : "";
            if (huorat.Any()) status += " H:";
            foreach (Player p in huorat)
            {
                status += " " + p.Name;
            }
            return status;
        }
    }
}