using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SubsAPI.Data;
using SubsAPI.DTO;
using SubsAPI.Helpers;
using SubsAPI.Models;
using SubsAPI.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class SubscriptionService : BaseService, ISubscriptionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TokenLenght _tokenLenght;

        public SubscriptionService(ApplicationDbContext dbContext, IOptions<TokenLenght> tokenLenght)
        {
            _dbContext = dbContext;
            _tokenLenght = tokenLenght.Value;
        }

        public async Task<BaseResponse<SubscribeResponseDto>> Subscribe(SubscribeDto model)
        {
            var result = new BaseResponse<SubscribeResponseDto>();

            var checkActiveSubscription = await _dbContext.Subscribers
                .FirstOrDefaultAsync(x => x.ServiceId == model.ServiceId
                && x.PhoneNumber == model.PhoneNumber &&
                x.Status == SubscriptionStatus.Active);

            if(checkActiveSubscription != null)
            {
                Errors.Add("User is already subscribed");
                result.ResponseMessage = "User is already subscribed";
                return new BaseResponse<SubscribeResponseDto>(result.ResponseMessage, Errors);
            }

            var newSubscription = new Subscriber
            {
                Status = SubscriptionStatus.Active,
                ServiceId = model.ServiceId,
                PhoneNumber = model.PhoneNumber,
                SubscribeDate = DateTime.Now,
                SubscriptionId = GenerateSubscriptionId(_tokenLenght.Subscription)
            };

            await _dbContext.Subscribers.AddAsync(newSubscription);
            await _dbContext.SaveChangesAsync();

            result.Data = new SubscribeResponseDto
            {
                DateSubscribed = newSubscription.SubscribeDate,
                PhoneNumber = model.PhoneNumber,
                ServiceId = model.ServiceId,
                Status = newSubscription.Status,
                SubscriptionId = newSubscription.SubscriptionId
            };

            result.ResponseMessage = "User is subscribed";
            return result;
        }

        

        public async Task<BaseResponse<UnSubscribeResponseDto>> UnSubscribe(UnSubscribeDto model)
        {
            var result = new BaseResponse<UnSubscribeResponseDto>();

            var checkActiveSubscription = await _dbContext.Subscribers
                .FirstOrDefaultAsync(x => x.ServiceId == model.ServiceId
                && x.PhoneNumber == model.PhoneNumber &&
                x.Status == SubscriptionStatus.Active);

            if (checkActiveSubscription == null)
            {
                result.ResponseMessage = "User is not subscribed";
                return new BaseResponse<UnSubscribeResponseDto>(result.ResponseMessage, Errors);
            }

            checkActiveSubscription.Status = SubscriptionStatus.InActive;
            checkActiveSubscription.UnsubscribeDate = DateTime.Now;

            _dbContext.Subscribers.Update(checkActiveSubscription);
            await _dbContext.SaveChangesAsync();


            result.Data = new UnSubscribeResponseDto
            {
                DateUnSubscribed = checkActiveSubscription.UnsubscribeDate,
                PhoneNumber = model.PhoneNumber,
                ServiceId = model.ServiceId,
                Status = checkActiveSubscription.Status                
            };

            result.ResponseMessage = "User unsubscribed";

            return result;
        }
    }
}
