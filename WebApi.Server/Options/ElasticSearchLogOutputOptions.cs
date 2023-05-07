using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Параметры подключения к Elasticsearch для записи логов.
/// </summary>
public class ElasticSearchLogOutputOptions
{
  /// <summary>
  /// Адрес сервиса Elasticsearch.
  /// </summary>
  [LoggableConfigurationValue]
  public string? ServiceAddress { get; set; }

  /// <summary>
  /// Имя индекса Elasticsearch для логов.
  /// </summary>
  [LoggableConfigurationValue]
  public string? Index { get; set; }

  /// <summary>
  /// Идентификатор API ключа.
  /// </summary>
  public string? ApiKeyId { get; set; }

  /// <summary>
  /// API ключ.
  /// </summary>
  public string? ApiKey { get; set; }
}
