using System;
using System.Collections.Generic;

#nullable disable

namespace Drink.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<UserFavorite> Favorites { get; set; }
        public List<UserIngredient> Ingredients { get; set; }
        
        public User()
        {

        }
        public User(string userName, string passWord)
        {
            this.Username = userName;
            this.Password = passWord;
            this.Favorites = null;
            this.Ingredients = null;
        }
    }
    

}
