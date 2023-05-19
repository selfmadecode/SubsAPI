using Microsoft.AspNetCore.Mvc;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        
        protected IActionResult ReturnResponse(dynamic model)
        {
            if (model.Status == RequestExecution.Successful)
            {
                return Ok(model);
            }

            return BadRequest(model);
        }

        protected DateTime CurrentDateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            rsp.Status = RequestExecution.Error;
#if DEBUG
            rsp.Errors = new List<string>() { $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}" };
            return Ok(rsp);
#else
            rsp.Errors = new List<string>() { "An error occurred while processing your request!" };
            return BadRequest(rsp);
#endif
        }
    }
}
