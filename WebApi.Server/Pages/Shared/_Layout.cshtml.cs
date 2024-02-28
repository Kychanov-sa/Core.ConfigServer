using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Pages.Shared;

/// <summary>
/// Базовая модель данных страницы.
/// </summary>
[Authorize]
public class BasePageModel : PageModel
{
  public bool IsAuthenticated { get; set; }
  public string? CurrentUserName { get; set; }

  public string? CurrentUserShortName { get; set; }

  public Uri SignOut 

  /// <summary>
  /// Конструктор.
  /// </summary>
  public BasePageModel(IOptions<Options.AuthenticationOptions> authenticationOptions)
  {
    IsAuthenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
    if (IsAuthenticated)
    {
      CurrentUserName = HttpContext.User.Identity?.Name;
      CurrentUserShortName = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("preferred_name")?.Value;
    }
  }
}
