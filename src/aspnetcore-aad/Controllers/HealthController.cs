using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_aad.Controllers
{
    [AllowAnonymous]
    public class HealthController : Controller
    {
        // GET: HealthController
        public ActionResult Index()
        {
            return Ok($"v{GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
        }
    }
}
