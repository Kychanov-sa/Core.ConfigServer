using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Services.Variables;

/// <summary>
/// Проверка здоровья сервиса переменных.
/// </summary>
internal class VariableServiceHealthCheck : IHealthCheck
{
  /// <summary>
  /// Конфигурация.
  /// </summary>
  private readonly IConfiguration _configuration;

  /// <summary>
  /// Сервис переменных.
  /// </summary>
  private readonly VariableService _variableService;


  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="configuration">Конфигурация.</param>
  /// <param name="variableService">Сервис переменных.</param>
  public VariableServiceHealthCheck(IConfiguration configuration, VariableService variableService)
  {
    _configuration = configuration;
    _variableService = variableService;
  }

  #region IHealthCheck

  /// <summary>
  /// <see cref="IHealthCheck.CheckHealthAsync(HealthCheckContext, CancellationToken)"/>
  /// </summary>
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    if (_variableService == null)
      return HealthCheckResult.Healthy();

    try
    {
      await _variableService.CheckHealth();
      return HealthCheckResult.Healthy();
    }
    catch (Exception ex)
    {
      return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
    }
  }

  #endregion
}
