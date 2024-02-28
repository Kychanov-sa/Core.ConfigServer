using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Настройки доверенного издателя токенов.
/// </summary>
public class TrustedTokenIssuerOptions
{
  /// <summary>
  /// Имя издателя токена.
  /// </summary>
  [LoggableConfigurationValue]
  public string? Issuer { get; set; }

  /// <summary>
  /// Ключ шифрования токенов.
  /// </summary>
  public string? EncryptionKey { get; set; }

  /// <summary>
  /// Отпечаток сертификата для проверки аутентичности токенов безопасности.
  /// </summary>
  [LoggableConfigurationValue]
  public string? SigningCertificateThumbprint { get; set; }

  /// <summary>
  /// Путь к файлу *.crt сертификата для проверки аутентичности токенов безопасности.
  /// </summary>
  [LoggableConfigurationValue]
  public string? SigningCertificatePath { get; set; }
}
