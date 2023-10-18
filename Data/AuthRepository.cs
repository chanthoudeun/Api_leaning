using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api_leaning.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.MyUser.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(userName.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHas, user.PasswordSet))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;

        }

        public async Task<ServiceResponse<int>> Register(MyUser user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if (await UserExists(user.UserName))
            {
                response.Success = false;
                response.Message = "User already exist";
                return response;
            }
            CreatedPasswordHash(password, out byte[] PasswordHas, out byte[] PasswordSet);
            user.PasswordHas = PasswordHas;
            user.PasswordSet = PasswordSet;
            _context.MyUser.Add(user);
            await _context.SaveChangesAsync();

            response.Data = user.Id;
            return response;

        }


        public async Task<bool> UserExists(string userName)
        {
            if (await _context.MyUser.AnyAsync(c => c.UserName.ToLower() == userName.ToLower()))
            {
                return true;
            }

            return false;

        }


        private void CreatedPasswordHash(string password, out byte[] PasswordHas, out byte[] PasswordSet)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSet = hmac.Key;
                PasswordHas = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHas, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHas);
            }
        }

        private string CreateToken(MyUser user)
        {
            List<Claim> claims = new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Name, user.UserName),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}