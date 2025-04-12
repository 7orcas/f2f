using Backend.App.Machines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security;

namespace Backend.Base
{
    public class InterceptorFilter : ActionFilterAttribute
    {
        private readonly TokenServiceI _tokenService;
        private readonly SessionServiceI _sessionService;
        private readonly PermissionServiceI _permissionService;

        public InterceptorFilter(
            TokenServiceI tokenService,
            SessionServiceI sessionService,
            PermissionServiceI permissionService)
        {
            _tokenService = tokenService;
            _sessionService = sessionService;
            _permissionService = permissionService;
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

            var session = null as SessionEnt;

            // Extract the token and get session
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                var token = authorizationHeader.Substring("Bearer".Length);
                var tv = _tokenService.DecodeToken(token);
                session = _sessionService.GetSession(tv.SessionKey);
                context.HttpContext.Items["session"] = session;
            }

            //Test permissions
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                //Method assign permission
                MethodInfo methodInfo = controllerActionDescriptor.MethodInfo;
                var perm = methodInfo.GetCustomAttribute<PermissionAtt>();
                var crud = methodInfo.GetCustomAttribute<CrudAtt>();

                //Class assign permission
                if (perm == null)
                {
                    Type controllerType = controllerActionDescriptor.ControllerTypeInfo.AsType();
                    perm = controllerType.GetCustomAttribute<PermissionAtt>();
                }

                if (!_permissionService.IsAuthorizedCall(session, perm, crud))
                {
                    var r = new _ResponseDto
                    {
                        Valid = false,
                        ErrorMessage = "Not Authorised",  //ToDo label
                        StatusCode = 403 // HTTP status code
                    };
                    context.Result = new OkObjectResult(r);
                }
            }

            

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Intercept after the controller action executes
        }
    }
}

