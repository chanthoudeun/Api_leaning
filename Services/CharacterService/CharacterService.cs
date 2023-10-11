using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Data;
using Api_leaning.Dtos.Character;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api_leaning.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {



        public CharacterService(IMapper mapper, DataContext context)
        {
            Mapper = mapper;
            Context = context;
        }

        public IMapper Mapper { get; }
        public DataContext Context { get; }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = Mapper.Map<Character>(newCharacter);
            Context.Characters.Add(character);
            await Context.SaveChangesAsync();
            serviceResponse.Data = await Context.Characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await Context.Characters.FirstAsync(c => c.Id == id);
                Context.Characters.Remove(character);
                await Context.SaveChangesAsync();
                response.Data = Context.Characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {

            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await Context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacter.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbcharacter = await Context.Characters.FirstOrDefaultAsync(e => e.Id == id);
            serviceResponse.Data = Mapper.Map<GetCharacterDto>(dbcharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await Context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);

                // anther way to add variable update

                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intelligence = updateCharacter.Intelligence;
                character.Class = updateCharacter.Class;
                await Context.SaveChangesAsync();
                response.Data = Mapper.Map<GetCharacterDto>(character);

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