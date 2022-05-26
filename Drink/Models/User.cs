using System;
using System.Collections.Generic;

#nullable disable

namespace Drink.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
