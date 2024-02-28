using GlacialBytes.Core.ConfigServer.WebApi.Server.Models;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Pages.Shared;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Pages;

/// <summary>
/// Модель данных страницы профиля.
/// </summary>
[Authorize()]
public class ProfileModel : BasePageModel
{
  /// <summary>
  /// Сервис переменных.
  /// </summary>
  private readonly IVariableService _variableService;

  /// <summary>
  /// Имя профиля.
  /// </summary>
  public string ProfileName { get; set; } = String.Empty;

  /// <summary>
  /// Переменные профиля.
  /// </summary>
  public IEnumerable<Variable> Variables { get; set; } = new List<Variable>();

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="variableService">Сервис переменных..</param>
  public ProfileModel(IVariableService variableService)
  {
    _variableService = variableService;
  }

  /// <summary>
  /// Метод обработки события запроса страницы.
  /// </summary>
  public async Task OnGet(string profileId)
  {
    var variables = await _variableService.GetVariables(profileId);

    Variables = from v in variables
                select new Variable()
                {
                  Name = v.Key,
                  Value = v.Value?.ToString(),
                  Type = v.Value?.GetType() ?? typeof(Object),
                };
    ProfileName = profileId;
  }
}
