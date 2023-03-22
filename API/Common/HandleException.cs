using Microsoft.AspNetCore.Mvc;

namespace API.Common;

public static class HandleExceptions
{
    public static ActionResult OnError(Exception ex)
    {
        return ex.InnerException is not null
            ? new BadRequestObjectResult(new { code = 400, message = ex.InnerException.Message, error = "Bad Request" })
            : new BadRequestObjectResult(new { code = 400, message = ex.Message, error = "Bad Request" });
    }
}