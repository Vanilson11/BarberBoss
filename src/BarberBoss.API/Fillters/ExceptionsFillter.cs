using BarberBoss.Communication.Responses;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberBoss.API.Fillters;

public class ExceptionsFillter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is BarberBossException)
        {
            HandleProjectExeption(context);
        }
        else
        {
            UnknowError(context);
        }
    }

    private void HandleProjectExeption(ExceptionContext context)
    {
        var barberBossException = (BarberBossException)context.Exception;
        var errorMessagesResponse = new ResponseErrorMessagesJson(barberBossException.GetErrors());

        context.HttpContext.Response.StatusCode = barberBossException.StatusCode;
        context.Result = new ObjectResult(errorMessagesResponse);
    }

    private void UnknowError(ExceptionContext context)
    {
        var errorResponseMessage = new ResponseErrorMessagesJson(ResourceErrorMessages.UNKNOW_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponseMessage);
    }
}
