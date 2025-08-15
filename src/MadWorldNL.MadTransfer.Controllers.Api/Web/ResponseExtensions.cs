using LanguageExt.Common;
using MadWorldNL.MadTransfer.Default;
using MadWorldNL.MadTransfer.Exceptions;

namespace MadWorldNL.MadTransfer.Web;

public static class ResponseExtensions
{
    public static IResult ToFaultyResult(this Error error)
    {
        return error.Exception.Match(
            exception => exception.ToFaultyResult(), 
            Results.InternalServerError);
    }

    private static IResult ToFaultyResult(this Exception exception)
    {
        return exception switch {
            NotFoundException => Results.NotFound(),
            ParseException parseException => parseException.ToBadRequest(),
            _ => Results.InternalServerError()
        };       
    }

    private static IResult ToBadRequest(this ParseException exception)
    {
        return Results.BadRequest(new BadRequestResponse()
        {
            ErrorCode = exception.ErrorCode.ToString(),
            ErrorMessage = exception.Message
        });       
    }
}