using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EbookUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/500")]
        public IActionResult Index()
        {
            var exceptionData = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionData != null)
            {
                ViewBag.ErrorMessage = exceptionData.Error.Message;
                ViewBag.RouteOfException = exceptionData.Path;
            }
            return View("HandleErrorCode");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 264:
                    ViewBag.ErrorMessage = "We couldn't find anything try different or less specific keywords.";   
                    break;
            }

            return View();
        }
    }
}