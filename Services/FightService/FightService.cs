using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Data;
using Api_leaning.Dtos.Fight;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api_leaning.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto(),
            };

            try
            {

                var characters = await _context.Characters
                          .Include(w => w.Weapon)
                          .Include(s => s.Skills)
                          .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();
                bool defeated = false;
                while (!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool uesWeapon = new Random().Next(2) == 0;
                        if (uesWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} Damage");
                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeats!");
                            response.Data.Log.Add($"{attacker.Name} win with {attacker.HitPoints} HP left!");
                            break;
                        }

                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == skillAttack.AttackerId);
                var opponent = await _context.Characters.Include(c => c.Weapon)
                               .FirstOrDefaultAsync(c => c.Id == skillAttack.OpponentId);
                var skill = attacker.Skills.FirstOrDefault(c => c.Id == skillAttack.SkillId);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = $"{skill.Name} don't know that skill";
                    return response;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);
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

        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
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
                int damage = DoWeaponAttack(attacker, opponent);
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

        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HightScoreDto>>> GetHighScore()
        {
            var character = await _context.Characters
            .Where(c => c.Fights > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats).ToListAsync();
            var response = new ServiceResponse<List<HightScoreDto>>
            {
                Data = character.Select(c => _mapper.Map<HightScoreDto>(c)).ToList()
            };
            return response;
        }
    }
}