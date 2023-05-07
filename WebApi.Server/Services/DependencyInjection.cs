using GlacialBytes.Core.ConfigServer.WebApi.Server.Services.Variables;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Services;

/// <summary>
/// Инжектор зависимостей.
/// </summary>
public static class DependencyInjection
{
  /// <summary>
  /// Добавляет в зависимости серверы ресурсов.
  /// </summary>
  /// <param name="services">Коллекция служб.</param>
  /// <returns>Интерфейс коллекции служб.</returns>
  /// <typeparam name="TUser">Тип учётной записи пользователя.</typeparam>
  public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    Action<ApplicationOptions> setupOptions)
  {
    var options = new ApplicationOptions();
    setupOptions?.Invoke(options);
    VariableService.Configure(options.DatabaseConnectionString);

    services.AddSingleton<IVariableService>(new VariableService());
    return services;
  }
}
