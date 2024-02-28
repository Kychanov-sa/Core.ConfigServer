using System.ComponentModel.DataAnnotations;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Options;

/// <summary>
/// Общие настройки сервера.
/// </summary>
public class GeneralOptions
{
  /// <summary>
  /// Адрес сервера.
  /// </summary>
  [LoggableConfigurationValue]
  [Required]
  public string ServerAddress { get; init; } = String.Empty;
}
