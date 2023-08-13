using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using StudandoApi.Data.Contexts;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

namespace SudyApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, SudyContext sudyContext)
        {
            if (!String.IsNullOrEmpty(httpContext.Request.Headers[HeaderNames.Authorization]))
            {
                string formatToken = Token.FormatToken(httpContext.Request.Headers[HeaderNames.Authorization]);

                if (!Token.ValidateToken(formatToken))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync("");
                    return;
                }

                JwtSecurityToken json = Token.ReadToken(formatToken);
                UserLogged.UserId = Convert.ToInt32(json.Claims.First(claim => claim.Type == nameof(ClaimSudyType.UserId)).Value);
                UserModel? userLogged = sudyContext.Users.AsNoTracking().FirstOrDefault(x => x.UserId == UserLogged.UserId);

                if (userLogged == null || userLogged.Token == null)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync(string.Empty);
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}
