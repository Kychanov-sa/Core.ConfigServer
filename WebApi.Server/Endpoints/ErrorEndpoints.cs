using System.Reflection;
using Amazon.Runtime.Internal;
using GlacialBytes.Core.ConfigServer.WebApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки API для работы с данными версии сервиса.
/// </summary>
public static class ErrorEndpoints
{
  /// <summary>
  /// Добавляет конечные точки для работы с ошибками.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  public static void MapErrorEndpoints(this WebApplication app)
  {
    app.MapGet("/error", (HttpContext context) =>
    {
      var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
      var exception = exceptionHandler?.Error;

      bool isDevelopmentEnvironment = "Development".Equals(app.Environment.EnvironmentName);
      //if (exception != null)
      //  return HandleException(exceptionHandler, exception, isDevelopmentEnvironment);

      //return new JsonResult(FailureResult.UndefinedError);
      //ServiceVersion = AssemblyVersion ?? "1.0.0.0",
    });
  }
}
