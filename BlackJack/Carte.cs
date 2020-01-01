using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Carte
    {
        public int valeur { get; set; }

        public string path { get; set; }

        public Carte(int pValeur)
        {
            valeur = pValeur;
            path = valeur + ".png";
        }
    }
}
