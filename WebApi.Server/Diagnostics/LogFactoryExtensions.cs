using GlacialBytes.Core.ConfigServer.WebApi.Server.Options;
using NLog;
using NLog.Targets;
using NLog.Targets.ElasticSearch;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

/// <summary>
/// Конфигурация NLog.
/// </summary>
public static class NLogConfiguration
{
  /// <summary>
  /// Применяет конфигурацию к логгеру NLog.
  /// </summary>
  /// <param name="configuration">Конфигурация.</param>
  public static void ApplyConfiguration(IConfiguration configuration)
  {
    var rule = LogManager.Configuration.FindRuleByName("all");

    var minLevel = configuration.GetValue<string>("Logging:LogLevel:Default");
    rule.SetLoggingLevels(NLog.LogLevel.FromString(minLevel), NLog.LogLevel.Off);

    var diagnosticsOptions = configuration.GetSection("Diagnostics").Get<DiagnosticsOptions>();
    var target = diagnosticsOptions.LogOutput switch
    {
      LogOutputTarget.Console => LogManager.Configuration.AllTargets.First(t => t.Name == "log-colored-console"),
      LogOutputTarget.ElasticSearch => GetElasticSearchTarget(diagnosticsOptions.ElasticSearchLogOutput),
      LogOutputTarget.File => GetFileTarget(diagnosticsOptions.FileLogOutput),
      _ => null,
    };
    if (target != null)
      rule.Targets.Add(target);

    LogManager.Configuration.Variables["ServiceName"] = "GlacialBytes.Core.ConfigServer";
    LogManager.ReconfigExistingLoggers();
  }

  /// <summary>
  /// Настраивает и возвращает таргет для записи логов в файл.
  /// </summary>
  /// <param name="options">Настройки.</param>
  /// <returns>Настроенный таргет.</returns>
  private static Target? GetFileTarget(FileLogOutputOptions? options)
  {
    options ??= new FileLogOutputOptions();
    FileTarget? target = options.Format switch
    {
      FileLogOutputFormat.Text => LogManager.Configuration.AllTargets.First(t => t.Name == "log-text-file") as FileTarget,
      FileLogOutputFormat.Json => LogManager.Configuration.AllTargets.First(t => t.Name == "log-json-file") as FileTarget,
      _ => null,
    };
    if (target != null && options.File != null)
      target.FileName = Path.Combine(options.Directory ?? "./", options.File);
    return target;
  }

  /// <summary>
  /// Настраивает и возвращает таргет для записи логов в ElasticSearch.
  /// </summary>
  /// <param name="options">Настройки.</param>
  /// <returns>Настроенный таргет.</returns>
  private static Target? GetElasticSearchTarget(ElasticSearchLogOutputOptions? options)
  {
    options ??= new ElasticSearchLogOutputOptions();
    var target = LogManager.Configuration.AllTargets.First(t => t.Name == "log-elastic-search") as ElasticSearchTarget;
    if (target != null)
    {
      target.ApiKeyId = options.ApiKeyId;
      target.ApiKey = options.ApiKey;
      target.Uri = options.ServiceAddress;
      target.Index = options.Index;
    }

    return target;
  }
}
