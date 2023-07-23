using System.Data.Common;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using GlacialBytes.Core.ConfigServer.WebApi.Server.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

[assembly: InternalsVisibleTo("WebApi.Test, PublicKey=00240000048000009400000006020000002400005253413100040000010001002500514a607b230e5731be00c334ce21fa0fcc7ab67c5b4df498c9eadc36375189a4c310222999dd227c6a606b715b7467d2d6d288bae7342272721142c3dd38d0ea89f27168df91ad18998ca6a6d9bbb992495f4160ccfdda6a3fb7ae654792c9c0859bce10342670c23d2992cd35c6377aa8efd74b868003ae5dc017117edc")]
namespace GlacialBytes.Core.ConfigServer.WebApi.Server.Services.Variables;

/// <summary>
/// Реализация сервиса переменных.
/// </summary>
internal class VariableService : IVariableService
{
  /// <summary>
  /// Имя коллекции с переменными в MongoDB.
  /// </summary>
  private const string MongoVariablesCollectionName = "vars";

  /// <summary>
  /// Адрес сервера MongoDB.
  /// </summary>
  private static string? _mongoServerAddress;

  /// <summary>
  /// Имя базы данных MongoDB.
  /// </summary>
  private static string? _mongoDatabaseName;

  /// <summary>
  /// Коллекция профилей с переменными.
  /// </summary>
  private readonly IMongoCollection<BsonDocument> _profilesCollection;

  /// <summary>
  /// Конструктор.
  /// </summary>
  public VariableService()
  {
    if (_mongoServerAddress == null)
      throw new ConfigurationException("Data source is empty.");
    if (_mongoDatabaseName == null)
      throw new ConfigurationException("Initial catalog is empty.");

    var mongoClient = new MongoClient(_mongoServerAddress);
    var mongoDatabase = mongoClient.GetDatabase(_mongoDatabaseName);
    _profilesCollection = mongoDatabase.GetCollection<BsonDocument>(MongoVariablesCollectionName);
  }

  /// <summary>
  /// Конфигурирует сервис.
  /// </summary>
  /// <param name="connectionString">Строка подключения.</param>
  public static void Configure(string connectionString)
  {
    Guard.Against.Null(connectionString, nameof(connectionString));

    var connectionStringBuilder = new DbConnectionStringBuilder()
    {
      ConnectionString = connectionString,
    };

    _mongoServerAddress = connectionStringBuilder["Data source"]?.ToString();
    _mongoDatabaseName = connectionStringBuilder["Initial catalog"]?.ToString();
  }

  #region IVariableService

  /// <summary>
  /// <see cref="IVariableService.GetProfiles"/>
  /// </summary>
  public async Task<IEnumerable<string>> GetProfiles()
  {
    var filter = new BsonDocument();
    var categoriesList = await _profilesCollection.DistinctAsync<string>("_id", filter);
    return categoriesList.ToEnumerable<string>();
  }

  /// <summary>
  /// <see cref="IVariableService.GetVariable(string, string)"/>
  /// </summary>
  public async Task<object?> GetVariable(string profile, string variable)
  {
    Guard.Against.Null(profile, nameof(profile));
    Guard.Against.Null(variable, nameof(variable));

    var filter = Builders<BsonDocument>.Filter.Eq("_id", profile);
    var profileDocument = await _profilesCollection.Find(filter).FirstOrDefaultAsync();
    if (profileDocument != null && profileDocument.TryGetValue(variable, out BsonValue value))
      return BsonTypeMapper.MapToDotNetValue(value);
    return null;
  }

  /// <summary>
  /// <see cref="IVariableService.GetVariables(string)"/>
  /// </summary>
  public async Task<IEnumerable<KeyValuePair<string, object?>>> GetVariables(string profile)
  {
    Guard.Against.Null(profile, nameof(profile));

    var filter = Builders<BsonDocument>.Filter.Eq("_id", profile);
    var profileDocument = await _profilesCollection.Find(filter).FirstOrDefaultAsync();
    if (profileDocument == null)
      return Enumerable.Empty<KeyValuePair<string, object?>>();
    return profileDocument.ToDictionary();
  }

  /// <summary>
  /// <see cref="IVariableService.SetVariable(string, string, object?)"/>
  /// </summary>
  public async Task SetVariable(string profile, string variable, object? value)
  {
    Guard.Against.Null(profile, nameof(profile));
    Guard.Against.Null(variable, nameof(variable));

    var filter = Builders<BsonDocument>.Filter.Eq("_id", profile);
    var update = Builders<BsonDocument>.Update.Set(variable, value);
    _ = await _profilesCollection.UpdateOneAsync(filter, update, new UpdateOptions()
    {
       IsUpsert = true,
    });
  }

  #endregion
}
