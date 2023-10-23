using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Dtos.Character;
using Api_leaning.Dtos.Skill;
using Api_leaning.Dtos.Weapon;
using AutoMapper;

namespace Api_leaning
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<Skill, GetSkillDto>();
        }
    }
}