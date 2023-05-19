using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubsAPI.DTO;
using SubsAPI.Helpers;
using SubsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<JwtResponseDTO>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            return ReturnResponse(await _auth.Login(login));
        }

    }
}
