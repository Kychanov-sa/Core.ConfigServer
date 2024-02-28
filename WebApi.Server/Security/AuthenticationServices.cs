using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Security;

/// <summary>
/// Сервисы аутентификации.
/// </summary>
public static class AuthenticationServices
{
  /// <summary>
  /// Добавляет сервисы для аутентификации в IdentitySerivice.
  /// </summary>
  /// <param name="services">Коллекция сервисов.</param>
  /// <param name="configureOptions">Опции конфигурации.</param>
  /// <returns>Дополненная коллекция сервисов.</returns>
  public static IServiceCollection AddIdentityServiceAuthentication(this IServiceCollection services, Action<IdentityServiceAuthenticationOptions> configureOptions)
  {
    var identityServiceOptions = new IdentityServiceAuthenticationOptions();
    configureOptions?.Invoke(identityServiceOptions);

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
      {
        // Настройки аутентификации по кукам.
        options.Cookie.Name = "ConfigServerAuth_Host";
        options.ExpireTimeSpan = identityServiceOptions.SessionCookieLifetime;
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ForwardSignIn = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ForwardDefaultSelector = context => CookieAuthenticationDefaults.AuthenticationScheme;

        options.Events = new CookieAuthenticationEvents()
        {
          OnRedirectToLogin = context =>
          {
            // Адрес возврата на сервер с учётом пути и параметров текущего запроса.
            var returnQuery = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
            var returnLocationBuilder = new UriBuilder(identityServiceOptions.ServerUrl)
            {
              Path = context.Request.Path,
              Query = returnQuery.ToString(),
            };

            // Адрес перенаправления на точку входа в сервисе идентификации
            var redirectLocationBuilder = new UriBuilder(identityServiceOptions.IdentityServiceUrl)
            {
              Path = "SignIn",
            };

            var redirectQuery = HttpUtility.ParseQueryString(redirectLocationBuilder.Query);
            redirectQuery["replyUrl"] = identityServiceOptions.AuthorizeEndpointUrl.ToString();
            redirectQuery["returnUrl"] = returnLocationBuilder.ToString();
            redirectQuery["audience"] = identityServiceOptions.Audience;
            redirectLocationBuilder.Query = redirectQuery.ToString();

            context.Response.Redirect(redirectLocationBuilder.ToString());
            return Task.CompletedTask;
          },
        };
      });

    return services;
  }
}
