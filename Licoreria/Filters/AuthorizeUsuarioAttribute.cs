using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Filters
{
    public class AuthorizeUsuarioAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                ITempDataProvider provider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
                var TempData = provider.LoadTempData(context.HttpContext);
                String action = context.RouteData.Values["action"].ToString();
                String controller = context.RouteData.Values["controller"].ToString();

                if(action == "Perfil")
                {
                    TempData["action"] = "TodosProductos";
                    TempData["controller"] = "Productos";
                } else
                {
                    TempData["action"] = action;
                    TempData["controller"] = controller;
                }
                provider.SaveTempData(context.HttpContext, TempData);

                context.Result = this.GetRoute("Login", "Identity");
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
