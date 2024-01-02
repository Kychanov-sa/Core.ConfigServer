using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки API для работы с переменными конфигураций.
/// </summary>
public static class VariablesApiEndpoints
{
  /// <summary>
  /// Добавляет конечные точки API.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  public static void MapVarsApiEndpoints(this WebApplication app)
  {
    app.MapGet(
      "/api/vars/{profile}",
      [SwaggerOperation("Возвращает переменные профиля")]
      async ([SwaggerParameter("Имя профиля")] string profile, IVariableService variableService) =>
      {
        var variables = await variableService.GetVariables(profile);
        return new Dictionary<string, object?>(variables);
      })
      .WithName("GetProfileVariables")
      .WithTags("variables");

    app.MapGet(
      "/api/vars/{profile}/{variable}",
      [SwaggerOperation("Возвращает значение переменной профиля")]
      async ([SwaggerParameter("Имя профиля.")] string profile, [SwaggerParameter("Имя переменной")] string variable, IVariableService variableService) =>
      {
        var value = await variableService.GetVariable(profile, variable);
        return value;
      })
      .WithName("GetProfileVariableValue")
      .WithTags("variables");

    app.MapPut(
      "/api/vars/{profile}/{variable}",
      [SwaggerOperation("Устанавливает значение переменной профиля")]
      async ([SwaggerParameter("Имя профиля.")] string profile, [SwaggerParameter("Имя переменной")] string variable, [SwaggerParameter("Значение переменной")] object? value, IVariableService variableService) =>
      {
        await variableService.SetVariable(profile, variable, value);
      })
      .WithName("SetProfileVariableValue")
      .WithTags("variables");
  }
}
