namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Цель вывода логов.
/// </summary>
public enum LogOutputTarget
{
  /// <summary>
  /// Файл.
  /// </summary>
  File = 0,

  /// <summary>
  /// Консоль.
  /// </summary>
  Console = 1,

  /// <summary>
  /// Elasticsearch.
  /// </summary>
  ElasticSearch = 2,
}
