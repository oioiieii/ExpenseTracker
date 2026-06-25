using backend.Models;
using backend.Models.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Controllers;

public static class ControllerResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, IResult<T> result)
    {
        if(result.IsSuccess) return controller.Ok(result.Value); 
        
        switch (result.ErrorCode)
        {
            case ErrorCode.NotFound:
                return controller.NotFound(result.ErrorMessage);
            case ErrorCode.InvalidArgument:
                return controller.BadRequest(result.ErrorMessage);
            case ErrorCode.InvalidOperation:
                return controller.Conflict(result.ErrorMessage);
            default:
                return controller.Problem(result.ErrorMessage);
        }
    }
    
    public static ActionResult ToActionResult(this ControllerBase controller, IResult result)
    {
        if(result.IsSuccess) return controller.Ok(); 
        
        switch (result.ErrorCode)
        {
            case ErrorCode.NotFound:
                return controller.NotFound(result.ErrorMessage);
            case ErrorCode.InvalidArgument:
                return controller.BadRequest(result.ErrorMessage);
            case ErrorCode.InvalidOperation:
                return controller.Conflict(result.ErrorMessage);
            default:
                return controller.Problem(result.ErrorMessage);
        }
    }
}