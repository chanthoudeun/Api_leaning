using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_leaning.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordSet { get; set; }
        public byte[] PasswordHas { get; set; }
    }
}