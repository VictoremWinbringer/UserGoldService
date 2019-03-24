using Microsoft.AspNetCore.Mvc;
using System;
using UserGoldService.Domain;
using UserGoldService.Entities;

namespace UserGoldService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public IActionResult MyGold([FromHeader] string token)
        {
            Result<decimal> result = _service.GetMyGold(token);
            return GetValue(result, r => Content(r.ToString()));

        }

        [HttpPut("gold/{count}")]
        public IActionResult AddGold(decimal count, [FromHeader] string token)
        {
            Result<bool> result = _service.AddGold(count, token);
            return GetValue(result, r => Ok());
        }

        [HttpPut("register/{userName}")]
        public IActionResult Register(string userName)
        {
            Result<string> result = _service.Register(userName);
            return GetValue(result, r => Content(r));
        }

        private IActionResult GetValue<T>(Result<T> result, Func<T, IActionResult> success)
        {
            switch (result.errorKind)
            {
                case ErrorKind.None:
                    return success(result.value);
                case ErrorKind.NotValidToken:
                    return Unauthorized();
                default:
                    return BadRequest(result.errorMessage);
            }
        }
    }
}
