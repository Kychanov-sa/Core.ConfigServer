using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки проверки здоровья сервера.
/// </summary>
public static class HealthCheckEndpoints
{
  /// <summary>
  /// Системные теги проверки.
  /// </summary>
  public static string[] SystemChecksTags = new[] { "SYSTEM" };

  /// <summary>
  /// Сервисные теги проверки.
  /// </summary>
  public static string[] ServicesCheckTags = new[] { "SERVICES" };

  /// <summary>
  /// Опции JSON сериализатора.
  /// </summary>
  private static JsonSerializerOptions _jsonSerializerOptions = new ()
  {
    WriteIndented = true,
    Converters =
    {
      new JsonStringEnumConverter(),
    },
  };

  /// <summary>
  /// Добавляет конечные точки проверки здоровья.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  public static void MapHealthChecksEndpoints(this WebApplication app)
  {
    app.UseHealthChecks("/ready", new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = HealthChecksResponseWriter, });
    app.UseHealthChecks("/health", new HealthCheckOptions() { Predicate = (check) => check.Tags.Intersect(SystemChecksTags).Any(), ResponseWriter = HealthChecksResponseWriter, });
  }

  /// <summary>
  /// Делегат для записи ответа HealthChecks.
  /// </summary>
  /// <param name="context">Http-контекст.</param>
  /// <param name="result">Результат проверки.</param>
  /// <returns>Асинхронная операция.</returns>
  private static Task HealthChecksResponseWriter(HttpContext context, HealthReport result)
  {
    context.Response.ContentType = "application/json";
    return context.Response.WriteAsync(JsonSerializer.Serialize(result, _jsonSerializerOptions));
  }
}