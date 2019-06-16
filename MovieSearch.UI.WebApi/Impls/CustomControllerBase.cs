using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Impls
{
    [Authorize]
    public class CustomControllerBase : Controller
    {
    }
}
