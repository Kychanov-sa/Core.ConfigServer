using GlacialBytes.Core.ConfigServer.WebApi.Server.Services.Variables;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

  /// <summary>
  /// Добавляет проверки здоровья сервиса.
  /// </summary>
  /// <param name="builder">Построитель проверок здоровья.</param>
  /// <param name="failureStatus">Статус нарушения проверки.</param>
  /// <param name="tags">Теги проверок.</param>
  /// <returns>Дополненный построитель проверок здоровья.</returns>
  public static IHealthChecksBuilder AddApplicationServicesHealthChecks(
    this IHealthChecksBuilder builder,
    HealthStatus? failureStatus,
    IEnumerable<string> tags)
  {
    return builder.AddCheck<VariableServiceHealthCheck>("Variable service check", failureStatus, tags);
  }
}
