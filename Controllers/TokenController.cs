using Microsoft.AspNetCore.Mvc;
using SubsAPI.DTO;
using SubsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController
    {
        private readonly ITokenConfigService _tokenConfig;
        public TokenController(ITokenConfigService tokenConfig)
        {
            _tokenConfig = tokenConfig;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return ReturnResponse(await _tokenConfig.GetTokenLenght());
        }

        [HttpPut]
        public async Task<IActionResult> Post([FromBody] UpsertToken token)
        {
            return ReturnResponse(await _tokenConfig.CreateOrUpdateTokenLenght(token));
        }
    }
}
