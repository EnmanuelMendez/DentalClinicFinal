using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DentalClinic.Helpers
{
    /// <summary>
    /// Atributo para proteger acciones que requieren autenticación en el backoffice
    /// </summary>
    public class AdminRequiredAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var adminLogged = context.HttpContext.Session.GetString("AdminLogged");

            if (string.IsNullOrEmpty(adminLogged) || adminLogged != "true")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
