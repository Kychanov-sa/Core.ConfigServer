namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Services;

/// <summary>
/// Опции приложения.
/// </summary>
public class ApplicationOptions
{
  /// <summary>
  /// Строка подклюдчения к базе данных.
  /// </summary>
  public string DatabaseConnectionString { get; set; } = String.Empty;
}
