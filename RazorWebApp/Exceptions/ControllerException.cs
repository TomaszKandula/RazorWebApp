using System;
using RazorWebApp.Shared.Dto;

namespace RazorWebApp.Exceptions
{
    public static class ControllerException
    {
        public static ErrorHandlerDto Handle(Exception AException) 
        {
            var LCode = AException.HResult.ToString();
            var LMessage = string.IsNullOrEmpty(AException.InnerException?.Message)
                    ? AException.Message
                    : $"{AException.Message} ({ AException.InnerException.Message}).";

            return new ErrorHandlerDto 
            { 
                ErrorCode = LCode,
                ErrorDesc = LMessage
            };
        }
    }
}
