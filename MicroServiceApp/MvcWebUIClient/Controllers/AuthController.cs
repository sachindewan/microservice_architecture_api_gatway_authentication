using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcWebUIClient.Models;
using Newtonsoft.Json;

namespace MvcWebUIClient.Controllers
{
    public class AuthController : Controller
    {
        HttpClient _httpClient;
        public IConfiguration _configuration { get; }

        public AuthController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration["AppKey:BaseAdderess"]);
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), userViewModel);
            }
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(userViewModel), Encoding.UTF8,"application/json");
            var result = await _httpClient.PostAsync("/loginService", stringContent);
            if (result.IsSuccessStatusCode)
            {
                var userData = result.Content.ReadAsStringAsync().Result;
                var userModel = JsonConvert.DeserializeObject<User>(userData);
                CookieOptions cookiesOptions = new CookieOptions();
                cookiesOptions.Expires = DateTimeOffset.Now.AddMinutes(120);
                cookiesOptions.Domain = "http://localhost:51704";
                HttpContext.Response.Cookies.Append("api-gateway-token",userModel.Token,cookiesOptions);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //await HttpContext.AuthenticateAsync("CookiesAuthentication");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal);
            }
            return RedirectToAction("Index","Home", new { area = "User" });
        }
    }
}
