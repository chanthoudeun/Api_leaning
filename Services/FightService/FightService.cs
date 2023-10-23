using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Data;
using Api_leaning.Dtos.Fight;
using Microsoft.EntityFrameworkCore;

namespace Api_leaning.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == weaponAttack.AttackId);
                var opponent = await _context.Characters.Include(c => c.Weapon)
                               .FirstOrDefaultAsync(c => c.Id == weaponAttack.OpponentId);
                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated";
                }

                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    AttackerName = attacker.Name,
                    Opponent = opponent.Name,
                    AttackHP = attacker.HitPoints,
                    OpponentHP = attacker.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}