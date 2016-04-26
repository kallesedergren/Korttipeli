using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Player
    {
        // luokan muuttujat 
        public string Name { get; set; }
        public bool kysymysMestari = false;
        public bool tauolla = false;
        // tehdään lista "huorista"
        public List<Player> huorat = new List<Player>();
        private int tauot = 0;

        // ominaisuus, joka kertoo montako taukoa pelaajalla on
        public int Tauot
        {
            get { return tauot; }
            set { tauot = value; }
        }

        // konstruktori, asetetaan nimi pelaajalle
        public Player(string name)
        {
            Name = name;
        }
        
        // lisätään pelaajalle tauko
        public void addTauko()
        {
            tauot++;
        }

        // vähennetään yksi tauko ja asetetaan pelaaja tauolle
        public void substractTauko()
        {
            tauolla = true;
            tauot--;
        }

        // palauttaa pelaajan tiedot
        public override string ToString()
        {
            return Name + getStatus();
        }

        // palauttaa pelaajan status, eli onko pelaaja kysymysmestari, onko "huoria" tahi onko taukoja käytettävissä
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