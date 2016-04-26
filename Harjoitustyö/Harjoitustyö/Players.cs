using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harjoitustyö
{
    public class Players
    {
        private List<Player> playersList = new List<Player>();
        private readonly int MAX_PLAYERS = 8;

        // ominaisuus joka palauttaa pelaajalistan
        public List<Player> PlayersList
        {
            get { return playersList; }
        }

        // tyhjentää pelaajalistan
        public void resetList()
        {
            playersList = new List<Player>();
        }

        // lisää pelaajan, jos pelaajamäärä ei maksimissa
        public void AddPlayer(Player player)
        {
            if(playersList.Count() < MAX_PLAYERS)
            {
                playersList.Add(player);
            }
        }

        // nollataan kaikkien pelaajien statukset
        public void resetStatus()
        {
            resetKysymysmestari();
            resetTauot();
            resetHuora();
        }

        // etsitään pelaaja nimen perusteella
        public Player getByName(string name)
        {
            foreach(Player p in playersList)
            {
                if(p.Name == name)
                {
                    return p;
                }
            }
            return null;
        }

        // poistetaan pelaajilta kysymysmestari status
        public void resetKysymysmestari()
        {
            foreach (Player p in playersList)
            {
                p.kysymysMestari = false;
            }
        }

        // poistetaan tauot pelaajilta
        public void resetTauot()
        {
            foreach (Player p in playersList)
            {
                p.Tauot = 0;
                p.tauolla = false;
            }
        }

        // tyhjennetään huoralista
        public void resetHuora()
        {
            foreach (Player p in playersList)
            {
                p.huorat = new List<Player>();
            }
        }
    }
}
