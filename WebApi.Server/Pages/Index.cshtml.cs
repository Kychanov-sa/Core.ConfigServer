using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Pages;

/// <summary>
/// Модель данных главной страницы.
/// </summary>
public class IndexModel : PageModel
{
  /// <summary>
  /// Сервис переменных.
  /// </summary>
  private readonly IVariableService _variableService;

  /// <summary>
  /// Профили конфигураций.
  /// </summary>
  public List<string> Profiles { get; set; } = new List<string>();

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="variableService">Сервис переменных..</param>
  public IndexModel(IVariableService variableService)
  {
    _variableService = variableService;
  }

  /// <summary>
  /// Метод обработки события запроса страницы.
  /// </summary>
  public async Task OnGet()
  {
    var profiles = await _variableService.GetProfiles();
    Profiles = profiles.ToList();
  }

  public void OnCreate()
  {

  }
}
