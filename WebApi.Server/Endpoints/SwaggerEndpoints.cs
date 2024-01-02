namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;

/// <summary>
/// Конечные точки для работы Swagger.
/// </summary>
public static class SwaggerEndpoints
{
  /// <summary>
  /// Добавляет сервисы, необходимые для работы Swagger.
  /// </summary>
  /// <param name="services">Коллекция сервисов.</param>
  /// <returns>Дополненная коллекция.</returns>
  public static IServiceCollection AddSwaggerServices(this IServiceCollection services, string appTitle, string appVersion)
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc(appVersion, new () { Title = appTitle, Version = appVersion });
      c.EnableAnnotations();
    });

    return services;
  }

  /// <summary>
  /// Добавляет конечные точки Swagger.
  /// </summary>
  /// <param name="app">Настраиваемое веб-приложение.</param>
  /// <param name="description">Описание.</param>
  /// <param name="useSwaggerUI">Признак включения использования интерфейса Swagger.</param>
  public static void MapSwaggerEndpoints(this WebApplication app, string description, bool useSwaggerUI)
  {
    if (app.Environment.IsDevelopment())
    {
      app.MapSwagger();
      if (useSwaggerUI)
      {
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", description));
      }
    }
  }
}
