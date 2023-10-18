using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_leaning.Data;
using Api_leaning.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Api_leaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }


        [HttpPost("register")]

        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {

            var response = await _authRepository.Register(
               new MyUser
               {
                   UserName = request.UserName
               }, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }


        [HttpPost("login")]

        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {

            var response = await _authRepository.Login(
             request.UserName, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

    }
}