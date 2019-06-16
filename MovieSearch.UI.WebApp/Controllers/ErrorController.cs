using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MovieSearch.UI.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Internal(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}