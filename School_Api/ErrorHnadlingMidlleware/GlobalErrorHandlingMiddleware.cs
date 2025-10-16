using Azure;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.SharedError;

namespace School_Api.ErrorHnadlingMidlleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger ;

        public GlobalErrorHandlingMiddleware(RequestDelegate request, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = request;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                // IF Request Not Found
                await EndPointNotFound(context);

            }
            catch (Exception ex)
            { //Handle the exception
              //1- set status code for Response
              //2- set content type for Response
              //3- set response body
              //4- return the respons


                var ErrorResponse = new ErrorDetails()
                {
                    Message = ex.Message
                };
                //1- Set Status Code
                var statuCode = ex switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    ValidationErrorsExecption =>validationErrors((ValidationErrorsExecption)ex, ErrorResponse),
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };
                context.Response.StatusCode = statuCode;
                //2 - Set Content Type
                context.Response.ContentType = "application/json";

                //set the body of response and return it
                ErrorResponse.StatusCode = statuCode;
                await context.Response.WriteAsJsonAsync(ErrorResponse);

            }
        }
        private static async Task EndPointNotFound(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                context.Response.ContentType = "application/json";
                var errorResponse = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"The requested{context.Request.Path}was not found."
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }

        }

        private static int  validationErrors(ValidationErrorsExecption ex,ErrorDetails errorDetails)
        {
            errorDetails.Errors =ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
    
}
