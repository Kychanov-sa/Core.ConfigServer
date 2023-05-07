namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Exceptions
{
  /// <summary>
  /// Некорректная настройка приложения.
  /// </summary>
  public class ConfigurationException : ApplicationException
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public ConfigurationException(string message)
      : base(message)
    {
    }
  }
}
