using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Presentation.Controllers
{
    public class BaseController : Controller //Login için BaseController işlem controllerları buradan türetildi. Eğer login yapılmadıysa logine yönlendirir.
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var tenantId = context.HttpContext.Request.Cookies["TenantId"];

            if (string.IsNullOrEmpty(tenantId))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }


            base.OnActionExecuting(context);
        }
    }
}
