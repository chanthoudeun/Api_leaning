using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_leaning.Dtos.Fight
{
    public class AttackResultDto
    {
        public string AttackerName { get; set; } = string.Empty;
        public string Opponent { get; set; } = string.Empty;
        public int OpponentHP { get; set; }
        public int AttackHP { get; set; }
        public int Damage { get; set; }


    }
}