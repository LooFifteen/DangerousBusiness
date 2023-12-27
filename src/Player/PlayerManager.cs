using System;
using System.Collections.Generic;
using System.Linq;
using GameNetcodeStuff;

namespace DangerousBusiness.Player;

public class PlayerManager
{
    internal PlayerManager()
    {

    }

    public ISet<Player> GetPlayersByUsername(string username)
    {
        return GetPlayersByPredicate(player => player.GetController().playerUsername == username);
    }

    public Player GetPlayerBySteamId(ulong steamId)
    {
        return GetPlayersByPredicate(player => player.GetController().playerSteamId == steamId).First();
    }

    public Player GetPlayerByController(PlayerControllerB controller)
    {
        return new Player(controller);
    }

    public ISet<Player> GetPlayersByPredicate(Predicate<Player> predicate)
    {
        return GetPlayers()
            .Where(player => predicate(player))
            .ToHashSet();
    }

    public ISet<Player> GetPlayers()
    {
        return StartOfRound.Instance.allPlayerScripts
            .Select(GetPlayerByController)
            .ToHashSet();
    }
}