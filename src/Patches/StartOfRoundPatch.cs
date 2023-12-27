using System.Collections.Generic;
using System.Linq;
using GameNetcodeStuff;
using HarmonyLib;

namespace DangerousBusiness.Patches;

[HarmonyPatch(typeof(StartOfRound))]
public class StartOfRoundPatch
{
    /// <summary>
    /// Allows the injection of custom player notes into the end-of-round statistics.
    /// </summary>
    /// <param name="__instance"></param>
    /// <returns></returns>
    [HarmonyPatch("WritePlayerNotes")]
    [HarmonyPrefix]
    private static bool WritePlayerNotes(ref StartOfRound __instance)
    {
        // get all player statistics
        var playerStatistics = __instance.gameStats.allPlayerStats;

        // create map of player to player statistics
        var players = new Dictionary<PlayerControllerB, PlayerStats>();
        for (var index = 0; index < playerStatistics.Length; ++index)
        {
            var player = __instance.allPlayerScripts[index];
            var statistics = playerStatistics[index];

            // set active player flag on certain conditions
            // this is required to allow vanilla end of game statistics to work
            statistics.isActivePlayer = player.disconnectedMidGame
                                        || player.isPlayerDead
                                        || player.isPlayerControlled;

            // skip player if they're not active
            if (!statistics.isActivePlayer) continue;

            players[player] = statistics;
        }

        var plugin = DangerousBusiness.GetInstance();

        // iterate through all player notes
        foreach (var note in plugin.GetPlayerNoteManager().GetPlayerNotes())
        {
            // sort the players
            var sortedPlayers = players.Keys.ToList();
            sortedPlayers.Sort(note);

            // add to player with the highest index
            var highestPlayer = sortedPlayers.Last();
            players[highestPlayer].playerNotes.Add(note.GetName());
        }

        // todo: include vanilla player notes
        return false; // never calculate vanilla player notes
    }
}