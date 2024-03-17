using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wabbajack.Common;
using Wabbajack.DTOs.JsonConverters;
using Wabbajack.Paths;
using Wabbajack.Paths.IO;

namespace Wabbajack.Services.OSIntegrated;

public class SettingsManager
{
    private readonly Configuration _configuration;
    private readonly DTOSerializer _dtos;
    private readonly ILogger<SettingsManager> _logger;

    public SettingsManager(ILogger<SettingsManager> logger, Configuration configuration, DTOSerializer dtos)
    {
        _logger = logger;
        _dtos = dtos;
        _configuration = configuration;
        _configuration.SavedSettingsLocation.CreateDirectory();
    }

    private AbsolutePath GetPath(string key)
    {
        return _configuration.SavedSettingsLocation.Combine(key).WithExtension(Ext.Json);
    }

    public async Task Save<T>(string key, T value)
    {
        var tmp = GetPath(key).WithExtension(Ext.Temp);
        await using (var s = tmp.Open(FileMode.Create, FileAccess.Write))
        {
            var opts = new JsonSerializerOptions(_dtos.Options)
            {
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync(s, value, opts);
        }

        await tmp.MoveToAsync(GetPath(key), true, CancellationToken.None);
    }

    public async Task<T> Load<T>(string key)
        where T : new()
    {
        var path = GetPath(key);
        try
        {
            if (path.FileExists())
            {
                await using (var s = path.Open(FileMode.Open))
                {
                    // cannot be bothered to figure out why DeserializeAsync gets stuck in BetterGameLocator,
                    // so we deserialize synchronously now
                    var d = JsonSerializer.Deserialize<T>(s, _dtos.Options)!;
                    return d;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Loading settings {Key}", key);
        }

        return new T();
    }
}