using LiteDB;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.LiteDb;

internal class SettingsLiteDbDataSource(ILiteDatabase liteDatabase, ILogger<SettingsLiteDbDataSource> logger) : LiteDbDataSource<Settings, string>(liteDatabase, logger), ISettingsDataSource
{
}