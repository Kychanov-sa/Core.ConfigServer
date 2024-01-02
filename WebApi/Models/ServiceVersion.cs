using System;
using System.Collections.Generic;
using System.Text;

namespace GlacialBytes.Core.ConfigServer.WebApi.Models;

/// <summary>
/// Модель данные о версии сервера.
/// </summary>
public class ServerVersion
{
  /// <summary>
  /// Версия сервиса конфигураций.
  /// </summary>
  public string? ServiceVersion { get; set;  }
}
