namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Security
{
  /// <summary>
  /// Опции аутентификации в сервисе идентификации.
  /// </summary>
  public class IdentityServiceAuthenticationOptions
  {
    /// <summary>
    /// Имя потребителя результатов аутентификации.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Адрес сервиса идентификации.
    /// </summary>
    public Uri IdentityServiceUrl { get; set; }

    /// <summary>
    /// Адрес конечной точки входа пользователя в сервисе идентификации.
    /// </summary>
    public Uri SignInEndpointUrl { get; set; }

    /// <summary>
    /// Адрес сервера конфигурации.
    /// </summary>
    public Uri ServerUrl { get; set; }

    /// <summary>
    /// Адрес конечной точки авторизации доступа.
    /// </summary>
    public Uri AuthorizeEndpointUrl { get; set; }

    /// <summary>
    /// Время жизни сессионной куки.
    /// </summary>
    public TimeSpan SessionCookieLifetime { get; set; }
  }
}
