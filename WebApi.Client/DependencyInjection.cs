using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GlacialBytes.Core.ConfigServer.WebApi.Client
{
  /// <summary>
  /// Инжектор зависимостей.
  /// </summary>
  public static class DependencyInjection
  {
    /// <summary>
    /// Добавляет в зависимости сервер конфигураций.
    /// </summary>
    /// <param name="services">Коллекция служб.</param>
    /// <returns>Построитель HTTP клиента.</returns>
    public static IHttpClientBuilder AddConfigServerApi(this IServiceCollection services)
    {
      return services.AddRefitClient<IConfigServer>(new RefitSettings()
      {
      });
    }
  }
}
