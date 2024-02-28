using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Exceptions;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Security;

/// <summary>
/// Валидатор JWT токенов.
/// </summary>
public class JwtTokenValidator
{
  /// <summary>
  /// Параметры валидации токена доверенного издателя.
  /// </summary>
  private readonly TokenValidationParameters _validationParameters;

  /// <summary>
  /// Конструктор.
  /// </summary>
  /// <param name="authenticationOptions">Опции аутентификации.</param>
  public JwtTokenValidator(IOptions<AuthenticationOptions> authenticationOptions)
  {
    var trustedIssuers = new List<string>();
    var trustedSecurityKeys = new List<SecurityKey>();

    foreach (var trustedIssuer in authenticationOptions.Value.TrustedIssuers)
    {
      if (!String.IsNullOrEmpty(trustedIssuer.Issuer))
      {
        try
        {
          SecurityKey? securityKey = null;
          if (!String.IsNullOrEmpty(trustedIssuer.SigningCertificatePath))
          {
            var certificate = new X509Certificate2(trustedIssuer.SigningCertificatePath);
            securityKey = new X509SecurityKey(certificate);
          }
          else if (!String.IsNullOrEmpty(trustedIssuer.SigningCertificateThumbprint))
          {
            var storeLocation = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
              StoreLocation.CurrentUser : StoreLocation.LocalMachine;
            using var store = new X509Store(StoreName.My, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, trustedIssuer.SigningCertificateThumbprint, true);
            if (certificates.Count == 0)
              throw new CertificateNotFoundException(trustedIssuer.SigningCertificateThumbprint);
            securityKey = new X509SecurityKey(certificates[0]);
            store.Close();
          }
          else if (!String.IsNullOrEmpty(trustedIssuer.EncryptionKey))
          {
            securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(trustedIssuer.EncryptionKey));
          }

          if (securityKey != null)
          {
            trustedIssuers.Add(trustedIssuer.Issuer);
            trustedSecurityKeys.Add(securityKey);
          }
        }
        catch (Exception ex)
        {
          Log.Exception(ex);
        }
      }
    }

    _validationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidIssuers = trustedIssuers,
      ValidateAudience = true,
      ValidAudience = authenticationOptions.Value.Audience,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKeys = trustedSecurityKeys,
      SaveSigninToken = true,
      ClockSkew = authenticationOptions.Value.TokenValidationClockSkew,
    };
  }

  /// <summary>
  /// Валидирует токен.
  /// </summary>
  /// <param name="token">Токен.</param>
  /// <returns>Принципал пользователя из токена.</returns>
  public ClaimsPrincipal ValidateToken(string token)
  {
    return new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out _);
  }
}