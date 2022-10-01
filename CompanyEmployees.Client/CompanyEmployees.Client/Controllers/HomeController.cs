using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompanyEmployees.Client.Models;
using CompanyEmployees.Client.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Auth.Flow.Models.Entities;

namespace CompanyEmployees.Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICompanyHttpClient _companyHttpClient;
        public HomeController(ICompanyHttpClient companyHttpClient)
        {
            _companyHttpClient = companyHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _companyHttpClient.GetClient();

            var response = await httpClient.GetAsync("test").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var weatherForecastViewModel = JsonConvert.DeserializeObject<List<WeatherForecast>>(responseString).ToList();

                return View(weatherForecastViewModel);
            }

            return RedirectToAction("Logout", "Account");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
