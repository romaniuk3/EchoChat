using InternshipChat.Domain.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace InternshipChat.Api.Extensions
{
    public static class ControllerExtension
    {
        public static ActionResult FromError(this ControllerBase controller, Error error)
        {
            switch (error.Code)
            {
                case ResultType.NotFound:
                    return controller.NotFound(error.Message);
                case ResultType.Invalid:
                    return controller.BadRequest(error.Message);
                case ResultType.Unathorized:
                    return controller.Unauthorized();
                default:
                    throw new Exception("Unhandled error has occured as a result of a service call.");
            }
        }
    }
}
