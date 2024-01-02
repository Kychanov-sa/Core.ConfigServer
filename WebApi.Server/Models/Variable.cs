namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Models;

public class Variable
{
  public string Name { get; set; }

  public string? Value { get; set; }

  public Type Type { get; set; }
}
