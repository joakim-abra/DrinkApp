using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class Favorite
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CocktailDbID { get; set; }

        public Favorite()
        {

        }
        public Favorite(string name, string cDB)
        {
            this.Name = name;
            this.CocktailDbID = cDB;
        }
    }
}
