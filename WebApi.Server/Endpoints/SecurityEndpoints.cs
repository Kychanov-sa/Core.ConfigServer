using System.Reflection;
using GlacialBytes.Core.ConfigServer.WebApi.Models;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Options;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки API для работы с данными версии сервиса.
/// </summary>
public static class SecurityEndpoints
{
  /// <summary>
  /// Добавляет конечные точки для работы с версией.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  public static void MapSecurityEndpoints(this WebApplication app)
  {
    app.MapPost(
     "/authorize",
     [AllowAnonymous] async (HttpContext context, JwtTokenValidator tokenValidator, IOptions<GeneralOptions> options) =>
     {
       string accessTokenType = context.Request.Form["accessTokenType"];
       string accessToken = context.Request.Form["accessToken"];
       string returnUrl = context.Request.Query["returnUrl"];

       if (String.IsNullOrEmpty(accessToken))
         throw new ArgumentException("Empty accessToken value is not allowed.");

       try
       {
         // Валидируем присланный токен и получаем принципал пользователя
         var principal = tokenValidator.ValidateToken(accessToken);
         await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

         string redirectUrl = String.IsNullOrEmpty(returnUrl) ? options.Value.ServerAddress : returnUrl;
         context.Response.Redirect(redirectUrl);
       }
       catch (SecurityTokenException)
       {
         context.Response.StatusCode = 401;
       }
     });
  }
}
