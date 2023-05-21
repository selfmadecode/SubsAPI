using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SubscriptionService> _log;

        public SubscriptionService(ILogger<SubscriptionService> log, ApplicationDbContext dbContext, IOptions<TokenLenght> tokenLenght)
        {
            _dbContext = dbContext;
            _tokenLenght = tokenLenght.Value;
            _log = log;
        }

        /// <summary>
        /// Subscribes a user to a service based on the provided <paramref name="model"/>.
        /// If the user already has an active subscription, a message is returned
        /// If the user is not subscribed, create a subscription for the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Error is user is already subscribed</returns>
        /// <returns>Success if a new subscription is created</returns>
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

            _log.LogInformation($"Created new subscription for {model.ServiceId} with {model.PhoneNumber}");

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

        /// <summary>
        /// Handles the process of unsubscribibg a user from a service, if an active subscription
        /// is not found, we return the error message "user is not subscribed"
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<BaseResponse<UnSubscribeResponseDto>> UnSubscribe(UnSubscribeDto model)
        {
            var result = new BaseResponse<UnSubscribeResponseDto>();

            // verify is a user has an active subscription
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

            _log.LogInformation($"Unsubscribed {model.ServiceId} on {model.PhoneNumber} at {checkActiveSubscription.UnsubscribeDate}");

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

        /// <summary>
        /// Retrieves the subscription status for a user
        /// based on the provided serviceId and phone number contained in the <paramref name="model"/>.
        /// </summary>
        /// <param name="model">The <see cref="CheckSubscriptionStatusDto"/> containing the necessary parameters.</param>
        /// <returns>User subscription status information.</returns>

        public async Task<BaseResponse<SubscriptionStatusDto>> Status(CheckSubscriptionStatusDto model)
        {
            var result = new BaseResponse<SubscriptionStatusDto>();

            // get user's subscription history for the same phone number and serviceId
            var subscriptions = _dbContext.Subscribers
                .Where(x => x.ServiceId == model.ServiceId
                && x.PhoneNumber == model.PhoneNumber);

            // if the subscription history is empty, then user is not subscribed
            if (!subscriptions.Any())
            {
                result.ResponseMessage = "User is not subscribed";
                return new BaseResponse<SubscriptionStatusDto>(result.ResponseMessage, Errors);
            }

            // User should only have a single active subscription in history
            var activeSubscription = await subscriptions.FirstOrDefaultAsync(x => x.Status == SubscriptionStatus.Active);

            if(activeSubscription != null)
            {
                result.Data = new SubscriptionStatusDto
                {
                    Status = activeSubscription.Status,
                    DateSubscribed = activeSubscription.SubscribeDate,
                    PhoneNumber = activeSubscription.PhoneNumber,
                    ServiceId = activeSubscription.ServiceId
                };

                result.ResponseMessage = "User is subscribed";
                return result;
            }

            // sort the subscription history in descending order using subscription date
            // the first subscription is the user's last subscription 
            var lastInactiveSubscription = await subscriptions
                .Where(x => x.Status == SubscriptionStatus.InActive)
                .OrderByDescending(x => x.SubscribeDate)
                .FirstOrDefaultAsync();

            result.Data = new SubscriptionStatusDto
            {
                Status = lastInactiveSubscription.Status,
                DateSubscribed = lastInactiveSubscription.SubscribeDate,
                PhoneNumber = lastInactiveSubscription.PhoneNumber,
                ServiceId = lastInactiveSubscription.ServiceId,
                DateUnSubscribed = lastInactiveSubscription.UnsubscribeDate
            };

            result.ResponseMessage = "User unsubscribed";
            return result;
        }

        /// <summary>
        /// Returns a user last 50 subscriptions if the token is valid
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="token"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<BaseResponse<IEnumerable<UserSubscriptionsDto>>> Subcriptions(string serviceId, string token, string phoneNumber)
        {
            var result = new BaseResponse<IEnumerable<UserSubscriptionsDto>>();

            var isTokenValid = await IsTokenValid(token, serviceId, _dbContext);

            if (!isTokenValid.Data)
            {
                Errors.AddRange(isTokenValid.Errors);
                result.ResponseMessage = isTokenValid.ResponseMessage;
                return result;
            }

            var subscriptions = _dbContext.Subscribers
                .Where(x => x.ServiceId == serviceId && x.PhoneNumber == phoneNumber)
                .OrderByDescending(x => x.SubscribeDate)
                .Take(50);

            var data = await subscriptions.Select(x => new UserSubscriptionsDto
            {
                ServiceId = x.ServiceId,
                DateSubscribed = x.SubscribeDate,
                PhoneNumber = x.PhoneNumber,
                DateUnSubscribed = x.UnsubscribeDate,
                SubscriptionId = x.SubscriptionId,
                Status = x.Status
            }).ToListAsync();

            result.Data = data;
            result.ResponseMessage = "Success";

            return result;
        }
    }
}
