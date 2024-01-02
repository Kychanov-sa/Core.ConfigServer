// <copyright file="Program.cs" company="GlacialBytes">
// Copyright (c) GlacialBytes. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Reflection;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Endpoints;
using NLog;
using NLog.Web;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
builder.Services.AddSwaggerServices("GlacialBytes ConfigServer", "v1");
builder.Services.AddRazorPages();
builder.Services.AddApplicationServices(opt =>
{
  opt.DatabaseConnectionString = builder.Configuration.GetConnectionString("Database");
});
builder.Services.AddHealthChecks()
  .AddApplicationServicesHealthChecks(HealthStatus.Unhealthy, HealthCheckEndpoints.SystemChecksTags);

// Собираем и запускаем приложение
var app = builder.Build();
app.UseExceptionHandler("/error");
app.MapHealthChecksEndpoints();
app.MapSwaggerEndpoints("GlacialBytes ConfigServer REST API v1", true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapVersionEndpoints();
app.MapVarsApiEndpoints();
app.Run();