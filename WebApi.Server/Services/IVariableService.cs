// <copyright file="IVariableService.cs" company="GlacialBytes">
// 2023 (C) GlacialBytes. All rights reserved.
// </copyright>

namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Services;

/// <summary>
/// Сервис переменных.
/// </summary>
public interface IVariableService
{
  /// <summary>
  /// Возвращает набор переменных для указанного профиля.
  /// </summary>
  /// <param name="profile">Имя профиля.</param>
  /// <returns>Коллекция переменных.</returns>
  Task<IEnumerable<KeyValuePair<string, object?>>> GetVariables(string profile);

  /// <summary>
  /// Возвращает значение переменной из указанного профиля.
  /// </summary>
  /// <param name="profile">Имя профиля.</param>
  /// <param name="variable">Имя переменной.</param>
  /// <returns>Значение переменной.</returns>
  Task<object?> GetVariable(string profile, string variable);

  /// <summary>
  /// Устанавливает значение переменной из указанного профиля.
  /// </summary>
  /// <param name="profile">Имя профиля.</param>
  /// <param name="variable">Имя переменной.</param>
  /// <param name="value">Значение переменной.</param>
  Task SetVariable(string profile, string variable, object? value);
}
