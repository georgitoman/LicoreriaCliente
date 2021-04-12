using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicoreriaCliente.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated || user.IsInRole("0"))
            {
                context.Result = this.GetRoute("AccesoDenegado", "Identity");
            }
        }

        public RedirectToRouteResult GetRoute(String action, String controller)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                action = action,
                controller = controller
            });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
