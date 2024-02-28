using GlacialBytes.Foundation.Dependencies;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server;

/// <summary>
/// Статический логгер.
/// </summary>
internal static class Log
{
  /// <summary>Логгер.</summary>
  private static readonly ILogger? _logger;

  /// <summary>
  /// Статический конструктор.
  /// </summary>
  static Log()
  {
    using var scope = ServiceActivator.GetScope();
    var factory = scope?.ServiceProvider.GetService<ILoggerFactory>();
    _logger = factory?.CreateLogger("SERVER");
  }

  /// <summary>
  /// Записывает в лог отладочное сообщение.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="parameters">Дополнительные параметры.</param>
  public static void Debug(string message, params object[] parameters)
  {
    _logger?.LogDebug(message, parameters);
  }

  /// <summary>
  /// Записывает в лог информационное сообщение.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="parameters">Дополнительные параметры.</param>
  public static void Info(string message, params object[] parameters)
  {
    _logger?.LogInformation(message, parameters);
  }

  /// <summary>
  /// Записывает в лог предупреждение.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="parameters">Дополнительные параметры.</param>
  public static void Warning(string message, params object[] parameters)
  {
    _logger?.LogWarning(message, parameters);
  }

  /// <summary>
  /// Записывает в лог ошибку.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="parameters">Дополнительные параметры.</param>
  public static void Error(string message, params object[] parameters)
  {
    _logger?.LogError(message, parameters);
  }

  /// <summary>
  /// Записывает в лог критическую ошибку.
  /// </summary>
  /// <param name="message">Текст сообщения.</param>
  /// <param name="parameters">Дополнительные парамеытры.</param>
  public static void CriticalError(string message, params object[] parameters)
  {
    _logger?.LogCritical(message, parameters);
  }

  /// <summary>
  /// Записывает в лог исключение.
  /// </summary>
  /// <param name="exception">Исключение.</param>
  /// <param name="parameters">Дополнительные параметры.</param>
  public static void Exception(Exception exception, params object[] parameters)
  {
    _logger?.LogError(exception, null, parameters);
  }
}
