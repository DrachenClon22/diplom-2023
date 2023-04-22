using diplomOriginal.Controllers;
using diplomOriginal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace diplomOriginal.Login
{
    public static class LoginManager
    {
        public static async Task<bool> Login(HttpContext context, LoginViewModel account)
        {
            Person? person = Database.GetPerson(account.Email, account.Password).Result;

            if (person is not null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, person.Name),
                    new Claim(ClaimTypes.Email, person.Email),
                    new Claim(ClaimTypes.Role, person.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await context.SignInAsync(claimsPrincipal);

                return true;
            }
            return false;
        }

        public static async Task<bool> Logout(HttpContext context)
        {
            try
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}