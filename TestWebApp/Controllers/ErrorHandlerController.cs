using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebApp.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErrorHandler
        public /*ActionResult*/string Index(Exception e)
        {
            return "An Error Occured." +e.Message;
            //return View();
        }

        public string NotFound()
        {
            return "Page Not Found! :(";
        }
    }
}