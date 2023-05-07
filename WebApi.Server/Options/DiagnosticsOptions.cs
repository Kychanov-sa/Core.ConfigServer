using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Настройки диагностики.
/// </summary>
public class DiagnosticsOptions
{
  /// <summary>
  /// Признак включения профайлинга запросов.
  /// </summary>
  [LoggableConfigurationValue]
  public bool EnableRequestProfiling { get; set; }

  /// <summary>
  /// Признак использования логирования конфигурации.
  /// </summary>
  [LoggableConfigurationValue]
  public bool EnableSettingsLogging { get; set; }

  /// <summary>
  /// Признак включения ведения журанала аудита.
  /// </summary>
  [LoggableConfigurationValue]
  public bool EnableAuditLogging { get; set; }

  /// <summary>
  /// Цель вывода логов.
  /// </summary>
  [LoggableConfigurationValue]
  public LogOutputTarget LogOutput { get; set; }

  /// <summary>
  /// Признак включения расширенного логирования.
  /// </summary>
  [LoggableConfigurationValue]
  public bool EnableExtendedLogOutput { get; set; }

  /// <summary>
  /// Параметры подключения к Elasticsearch для записи логов.
  /// </summary>
  [LoggableConfigurationValue]
  public ElasticSearchLogOutputOptions? ElasticSearchLogOutput { get; set; }

  /// <summary>
  /// Параметры для записи логов в файл.
  /// </summary>
  [LoggableConfigurationValue]
  public FileLogOutputOptions? FileLogOutput { get; set; }
}
