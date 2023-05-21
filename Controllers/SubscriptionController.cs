using Microsoft.AspNetCore.Mvc;
using SubsAPI.DTO;
using SubsAPI.Helpers;
using SubsAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscription;

        public SubscriptionController(ISubscriptionService subscription)
        {
            _subscription = subscription;
        }

        [HttpPost]
        [Route("subscribe")]
        [ProducesResponseType(typeof(BaseResponse<SubscribeResponseDto>), 200)]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeDto subscribe)
        {
            return ReturnResponse(await _subscription.Subscribe(subscribe));
        }

        [HttpPost]
        [Route("un-subscribe")]
        [ProducesResponseType(typeof(BaseResponse<UnSubscribeResponseDto>), 200)]
        public async Task<IActionResult> UnSubscribe([FromBody] UnSubscribeDto UnSubscribe)
        {
            return ReturnResponse(await _subscription.UnSubscribe(UnSubscribe));
        }

        [HttpGet]
        [Route("{serviceId}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<UserSubscriptionsDto>>), 200)]
        public async Task<IActionResult> Subscriptions([FromRoute] string serviceId, [FromQuery] string token, [FromQuery] string phoneNumber)
        {
            return ReturnResponse(await _subscription.Subsriptions(serviceId, token, phoneNumber));
        }

        [HttpPost]
        [Route("status")]
        [ProducesResponseType(typeof(BaseResponse<JwtResponseDTO>), 200)]
        public async Task<IActionResult> CheckStatus([FromBody] CheckSubscriptionStatusDto model)
        {
            return ReturnResponse(await _subscription.Status(model));
        }
    }
}
