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
using GlacialBytes.Foundation.Dependencies;
using GlacialBytes.Foundation.Web;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Options;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Security;

var builder = WebApplication.CreateBuilder(args);

// Конфигурируем логгер
builder.Logging.ClearProviders();
builder.WebHost.UseNLog();
NLogConfiguration.ApplyConfiguration(builder.Configuration);

// Готовим сервер к запуску
var assemblyVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
var logger = LogManager.GetLogger("SERVER");
logger.Info($"Application v.{assemblyVersion} starting...");
logger.Info($"Current culture: {Thread.CurrentThread.CurrentCulture.Name}");
logger.Info($"Current timezone: {TimeZoneInfo.Local.StandardName}");
logger.Info($"Processor count: {Environment.ProcessorCount}");
logger.Info($"OS version: {Environment.OSVersion}");

// Конфигурируем статический активатор сервисов
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
ServiceActivator.Configure(builder.Services.BuildServiceProvider());
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

// Добавляем опции в контейнер
builder.Services.AddOptions<GeneralOptions>()
  .BindConfiguration("General")
  .ValidateDataAnnotations()
  .ValidateOnStart();
builder.Services.AddOptions<AuthenticationOptions>()
  .BindConfiguration("Authentication")
  .ValidateDataAnnotations()
  .ValidateOnStart();

// Добавляем сервисы в контейнер
builder.Services.AddIdentityServiceAuthentication(options =>
{
  var generalOptions = builder.Configuration.GetSection("General").Get<GeneralOptions>();
  var authenticationOptions = builder.Configuration.GetSection("Authentication").Get<AuthenticationOptions>();
  var identityServiceCSBuilder = new WebServiceConnectionStringBuilder()
  {
    ConnectionString = builder.Configuration.GetConnectionString("IdentityService"),
  };

  var authorizeEndpointUriBuilder = new UriBuilder(generalOptions.ServerAddress ?? "/");
  authorizeEndpointUriBuilder.Path += "Authorize";

  options.ServerUrl = new Uri(generalOptions.ServerAddress ?? "/");
  options.IdentityServiceUrl = identityServiceCSBuilder.Address;
  options.SignInEndpointUrl = new Uri(identityServiceCSBuilder.Address, authenticationOptions.SignInEndpoint);
  options.Audience = authenticationOptions.Audience;
  options.AuthorizeEndpointUrl = authorizeEndpointUriBuilder.Uri;
  options.SessionCookieLifetime = authenticationOptions.SessionCookieLifetime;
});

builder.Services.AddSingleton<JwtTokenValidator>();
builder.Services.AddAuthorization();
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
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapVersionEndpoints();
app.MapVarsApiEndpoints();
app.MapErrorEndpoints();
app.MapSecurityEndpoints();
app.Run();