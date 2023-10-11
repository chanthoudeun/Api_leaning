using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_leaning.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public byte[] PasswordHas { get; set; }
        public byte[] PasswordSet { get; set; }

    }
}