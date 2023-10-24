using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api_leaning.Models
{
    public class MyUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordSet { get; set; }
        public byte[] PasswordHas { get; set; }

        public List<Character>? Characters { get; set; }

        [Required]
        public string Role { get; set; }
    }
}