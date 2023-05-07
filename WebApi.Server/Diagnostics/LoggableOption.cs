namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Diagnostics;

/// <summary>
/// Атрибут, определяющий параметр настроек, который может быть записан в лог.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LoggableConfigurationValueAttribute : Attribute
{
}
