using SubsAPI.DTO;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public interface ISubscriptionService
    {
        Task<BaseResponse<SubscribeResponseDto>> Subscribe(SubscribeDto model);
        Task<BaseResponse<UnSubscribeResponseDto>> UnSubscribe(UnSubscribeDto model);
        Task<BaseResponse<SubscriptionStatusDto>> Status(CheckSubscriptionStatusDto model);
        Task<BaseResponse<IEnumerable<UserSubscriptionsDto>>> Subcriptions(string serviceId, string token, string phoneNumber);
    }
}
