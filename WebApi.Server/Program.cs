// <copyright file="Program.cs" company="GlacialBytes">
// Copyright (c) GlacialBytes. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Конфигурируем логгер
builder.Logging.ClearProviders();
builder.WebHost.UseNLog();
NLogConfiguration.ApplyConfiguration(builder.Configuration);

// Готовим сервер к запуску
var assemblyVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
var logger = LogManager.GetCurrentClassLogger();
logger.Info($"Application v.{assemblyVersion} starting...");
logger.Info($"Current culture: {Thread.CurrentThread.CurrentCulture.Name}");
logger.Info($"Current timezone: {TimeZoneInfo.Local.StandardName}");
logger.Info($"Processor count: {Environment.ProcessorCount}");
logger.Info($"OS version: {Environment.OSVersion}");

// Добавляем сервисы в контейнер
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new () { Title = "GlacialBytes ConfigServer", Version = "v1" });
  c.EnableAnnotations();
});
builder.Services.AddApplicationServices(opt =>
{
  opt.DatabaseConnectionString = builder.Configuration.GetConnectionString("Database");
});

var app = builder.Build();

// Конфигурируем конвейер обработки HTTP запросов
if (app.Environment.IsDevelopment())
{
  app.MapSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlacialBytes ConfigServer REST API v1"));
}

app.UseHttpsRedirection();

// Конфигурируем REST API
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

app.Run();