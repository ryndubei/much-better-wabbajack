using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SteamKit2.GC.Dota.Internal;
using Wabbajack.Common;
using Wabbajack.Downloaders.GameFile;
using Wabbajack.DTOs;
using Wabbajack.Paths;

namespace Wabbajack.Services.OSIntegrated;

public class BetterGameLocator : GameLocator
{
    private const string ManuallyAddedGames = "manually-added-games";
    
    private readonly Dictionary<Game, AbsolutePath> _manuallyAddedGames;

    public BetterGameLocator(ILogger<GameLocator> logger, SettingsManager settingsManager) : base(logger)
    {
        _manuallyAddedGames = settingsManager.Load<Dictionary<Game, AbsolutePath>>(ManuallyAddedGames).Result;
        logger.LogInformation("Manually added games: {ManuallyAddedGames}", _manuallyAddedGames);
    }
    
    public override AbsolutePath GameLocation(Game game)
    {
        return _manuallyAddedGames.TryGetValue(game, out var overridePath) 
            ? overridePath 
            : base.GameLocation(game);
    }

    public override bool IsInstalled(Game game)
    {
        return _manuallyAddedGames.ContainsKey(game) || base.IsInstalled(game);
    }

    public override bool TryFindLocation(Game game, out AbsolutePath path)
    {
        return _manuallyAddedGames.TryGetValue(game, out path) || base.TryFindLocation(game, out path);
    }
}