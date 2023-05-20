using SubsAPI.DTO;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public interface ITokenConfigService
    {
        Task<BaseResponse<int>> GetTokenLenght();
        Task<BaseResponse<int>> CreateOrUpdateTokenLenght(UpsertToken token);
    }
}
