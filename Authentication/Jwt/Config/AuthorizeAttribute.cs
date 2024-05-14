using EFDatabaseModel.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectBase.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Jwt.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                //put auth logic in here
                var executionResult = new ExecutionResult<User>(
                    false,
                    "Authorization has been denied"
                );
                context.Result = new JsonResult(executionResult)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
