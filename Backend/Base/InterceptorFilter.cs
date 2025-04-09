using Backend.App.Machines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Base
{
    public class InterceptorFilter : ActionFilterAttribute
    {
        private readonly TokenServiceI _tokenService;
        private readonly SessionServiceI _sessionService;

        public InterceptorFilter(
            TokenServiceI tokenService,
            SessionServiceI sessionService)
        {
            _tokenService = tokenService;
            _sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
Console.WriteLine("calling inteceptor...");

            // Intercept the request before the controller action executes
            var interceptedObject = context.ActionArguments; // Access action arguments
                                                             // Add your interception logic here

            //Do Errors here
            //if (someCondition)
            //{
            //    context.Result = new ContentResult
            //    {
            //        Content = "Request intercepted",
            //        StatusCode = 403
            //    };
            //    return; // This prevents the controller action from executing
            //}

            //foreach (var argument in context.ActionArguments)
            //{
            //    Console.WriteLine($"Argument Key: {argument.Key}, Value: {argument.Value}");
            //}


            // Extract the token and get session
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                var token = authorizationHeader.Substring("Bearer".Length);
                var tv = _tokenService.DecodeToken(token);
                var session = _sessionService.GetSession(tv.SessionKey);
                context.HttpContext.Items["session"] = session;
            }

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Intercept after the controller action executes
        }
    }
}

