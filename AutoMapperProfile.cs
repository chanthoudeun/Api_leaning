using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Dtos.Character;
using AutoMapper;

namespace Api_leaning
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
        }
    }
}