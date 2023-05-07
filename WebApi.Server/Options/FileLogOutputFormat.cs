namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Формат вывода журнала в файл.
/// </summary>
public enum FileLogOutputFormat
{
  /// <summary>
  /// Текст.
  /// </summary>
  Text = 0,

  /// <summary>
  /// JSON.
  /// </summary>
  Json = 1,
}
