using System.Diagnostics;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Pages;

/// <summary>
/// Модель данных страницы ошибки.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : BasePageModel
{
  /// <summary>
  /// Идентификатор запроса.
  /// </summary>
  public string? RequestId { get; set; }

  /// <summary>
  /// Признак необходимости отображения идентификатора запроса.
  /// </summary>
  public bool ShowRequestId => !String.IsNullOrEmpty(RequestId);

  /// <summary>
  /// Логгер.
  /// </summary>
  private readonly ILogger<ErrorModel> _logger;

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="logger"> Логгер.</param>
  public ErrorModel(ILogger<ErrorModel> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Метод обработки события запроса страницы.
  /// </summary>
  public void OnGet()
  {
    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
  }
}
