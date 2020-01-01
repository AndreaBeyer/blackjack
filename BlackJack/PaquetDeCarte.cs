using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class PaquetDeCarte
    {
        Random random;
        public List<Carte> List { get; private set; }

        public PaquetDeCarte()
        {
            random = new Random();

            List = new List<Carte>();

            for(int i = 1; i < 53; i++)
            {
                List.Add(new Carte(i));
            }
        }

        public Carte getRandomCarte()
        {
            int index = random.Next(1, List.Count);
            Carte carte = List[index];
            List.Remove(carte);
            return carte;
        }
    }
}
