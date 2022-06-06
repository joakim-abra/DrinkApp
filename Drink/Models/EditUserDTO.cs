using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drink.Models
{
    public class EditUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int ID { get; set; }

        public EditUserDTO()
        {

        }
        public EditUserDTO(int id, string username, string password)
        {
            this.ID = id;
            this.Username = username;
            this.Password = password;
        
    }
    }

}
