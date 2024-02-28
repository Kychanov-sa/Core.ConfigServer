using System.ComponentModel.DataAnnotations;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Опции аутентификации.
/// </summary>
public class AuthenticationOptions
{
  /// <summary>
  /// Дефолтная погрешность проверки времени жизни токена.
  /// </summary>
  public static readonly TimeSpan DefaultTokenValidationClockSkew = TimeSpan.FromSeconds(5);

  /// <summary>
  /// Время жизни сессионной куки по умолчанию.
  /// </summary>
  public static readonly TimeSpan DefaultSessionCookieLifetime = TimeSpan.FromHours(1);

  /// <summary>
  /// Ресурс, запросивший аутентификацию.
  /// </summary>
  [LoggableConfigurationValue]
  [Required]
  public string Audience { get; init; } = "GlacialBytes.Core.ConfigServer";

  /// <summary>
  /// Доверенные издатели токенов доступа.
  /// </summary>
  [LoggableConfigurationValue]
  public TrustedTokenIssuerOptions[] TrustedIssuers { get; init; } = Array.Empty<TrustedTokenIssuerOptions>();

  /// <summary>
  /// Адрес возврата.
  /// </summary>
  public string? ReturnUrl { get; init; }

  /// <summary>
  /// Погрешность проверки времени жизни токена.
  /// </summary>
  public TimeSpan TokenValidationClockSkew { get; init; } = DefaultTokenValidationClockSkew;

  /// <summary>
  /// Адрес для входа в IdS.
  /// </summary>
  [LoggableConfigurationValue]
  [Required]
  public string SignInEndpoint { get; init; } = "SignIn";

  /// <summary>
  /// Время жизни сессионной куки.
  /// </summary>
  [LoggableConfigurationValue]
  [Required]
  public TimeSpan SessionCookieLifetime { get; init; } = DefaultSessionCookieLifetime;
}
