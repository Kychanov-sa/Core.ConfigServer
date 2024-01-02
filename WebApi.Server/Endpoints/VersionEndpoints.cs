using System.Reflection;
using GlacialBytes.Core.ConfigServer.WebApi.Models;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки API для работы с данными версии сервиса.
/// </summary>
public static class VersionEndpoints
{
  /// <summary>
  /// Версия сборки сервиса.
  /// </summary>
  public static string? AssemblyVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

  /// <summary>
  /// Добавляет конечные точки для работы с версией.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  public static void MapVersionEndpoints(this WebApplication app)
  {
    app.MapGet("/version", () => new ServerVersion()
    {
      ServiceVersion = AssemblyVersion ?? "1.0.0.0",
    });
  }
}
