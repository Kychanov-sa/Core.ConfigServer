using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace GlacialBytes.Core.ConfigServer.WebApi.Client
{
  /// <summary>
  /// Интерфейс API сервера конфигураций.
  /// </summary>
  public interface IConfigServer
  {
    /// <summary>
    /// Возвращает переменные профиля.
    /// </summary>
    /// <param name="profile">Имя профиля.</param>
    /// <returns>Переменные профиля.</returns>
    [Get("/api/vars/{profile}")]
    Task<Dictionary<string, object>> GetProfileVariables(string profile);

    /// <summary>
    /// Возвращает значение переменной профиля.
    /// </summary>
    /// <param name="profile">Имя профиля.</param>
    /// <param name="variable">Имя переменной.</param>
    /// <returns>Значение переменной.</returns>
    [Get("/api/vars/{profile}/{variable}")]
    Task<object> GetProfileVariableValue(string profile, string variable);

    /// <summary>
    /// Устанавливает значение переменной профиля.
    /// </summary>
    /// <param name="profile">Имя профиля.</param>
    /// <param name="variable">Имя переменной.</param>
    /// <param name="value">Значение переменной.</param>
    [Get("/api/vars/{profile}/{variable}")]
    Task SetProfileVariableValue(string profile, string variable, object value);

    /// <summary>
    /// Проверка жизнеспособности сервиса.
    /// </summary>
    /// <returns>Ответ на проверку жизнеспособности сервиса.</returns>
    [Get("/health")]
    Task<HttpResponseMessage> HealthCheck();

    /// <summary>
    /// Проверка готовности сервиса к работе.
    /// </summary>
    /// <returns>Ответ на проверку готовности сервиса.</returns>
    [Get("/ready")]
    Task<HttpResponseMessage> ReadyCheck();

    /// <summary>
    /// Возвращает версию сервиса.
    /// </summary>
    /// <returns>Номер версии сервиса.</returns>
    [Get("/version")]
    Task<string> GetVersion();
  }
}