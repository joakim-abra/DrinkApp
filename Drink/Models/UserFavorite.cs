using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class UserFavorite
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
        [ForeignKey("Favorite")]
        public int FavoriteID { get; set; }
        public Favorite Favorite { get; set; }
        
        public UserFavorite()
        {

        }
        public UserFavorite(User user, Favorite favorite)
        {
            this.User = user;
            this.Favorite = favorite;
        }
        public UserFavorite(int id, int userID,User user,int fID,Favorite favorite)
        {
            this.ID = id;
            this.UserID = userID;
            this.User = user;
            this.FavoriteID = fID;
            this.Favorite = favorite;
        }
    }
}
