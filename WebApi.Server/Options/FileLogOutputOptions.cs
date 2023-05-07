using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Параметры для записи логов в файл.
/// </summary>
public class FileLogOutputOptions
{
  /// <summary>
  /// Путь к папке для файлов лога.
  /// </summary>
  [LoggableConfigurationValue]
  public string? Directory { get; set; }

  /// <summary>
  /// Имя файла для логов.
  /// </summary>
  [LoggableConfigurationValue]
  public string? File { get; set; }

  /// <summary>
  /// Формат логов.
  /// </summary>
  [LoggableConfigurationValue]
  public FileLogOutputFormat Format { get; set; }
}
