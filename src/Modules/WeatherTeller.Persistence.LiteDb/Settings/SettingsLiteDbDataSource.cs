using LiteDB;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Settings;

namespace WeatherTeller.Persistence.LiteDb.Settings;

internal class SettingsLiteDbDataSource(ILiteDatabase liteDatabase, ILogger<SettingsLiteDbDataSource> logger) : LiteDbDataSource<Persistence.Settings.Settings, string>(liteDatabase, logger), ISettingsDataSource
{
}