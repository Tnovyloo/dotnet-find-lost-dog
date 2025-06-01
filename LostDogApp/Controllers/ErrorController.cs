
using Microsoft.AspNetCore.Mvc;

namespace LostDogApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.StatusCode = statusCode;

            return statusCode switch
            {
                400 => View("BadRequest"),
                401 => View("Unauthorized"),
                403 => View("Forbidden"),
                404 => View("NotFound"),
                _ => View("GenericError")
            };
        }
    }
}