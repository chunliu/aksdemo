using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnetcore_aad.Models;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace aspnetcore_aad.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["AKSClusterInfo"] = _configuration["AKSClusterInfo"];
            return View();
        }

        // Get: /GetSecretFromKV
        public async Task<IActionResult> GetSecretFromKV()
        {
            // Get the credential of user assigned identity
            var credential = new ChainedTokenCredential(
                new ManagedIdentityCredential(_configuration["UserAssignedIdentityClientId"]), 
                new AzureCliCredential());
            // Get secret from key vault
            var kvClient = new SecretClient(new Uri(_configuration["KeyVaultUri"]), credential);
            var secretBundle = await kvClient.GetSecretAsync(_configuration["SecretName"]);
            ViewData["KVSecret"] = secretBundle.Value.Name + ": " + secretBundle.Value.Value;

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
