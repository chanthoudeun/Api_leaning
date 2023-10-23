using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Dtos.Character;
using Api_leaning.Dtos.Weapon;

namespace Api_leaning.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}