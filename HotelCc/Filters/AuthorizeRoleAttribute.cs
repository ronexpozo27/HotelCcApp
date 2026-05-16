using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelCc.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public AuthorizeRoleAttribute(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            var user = httpContext.Session.GetString("Usuario");
            var role = httpContext.Session.GetString("Rol");

            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (!string.IsNullOrEmpty(_role) && role != _role)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}