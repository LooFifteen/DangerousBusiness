using System.Collections.Generic;
using System.Linq;
using DangerousBusiness.Event;
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
    private static bool PreWritePlayerNotes(ref StartOfRound __instance)
    {
        // get all player statistics
        var playerStatistics = __instance.gameStats.allPlayerStats;

        var plugin = DangerousBusiness.GetInstance();

        // create map of player to player statistics
        var players = new Dictionary<Player.Player, PlayerStats>();
        for (var index = 0; index < playerStatistics.Length; ++index)
        {
            var controller = __instance.allPlayerScripts[index];
            var player = plugin.GetPlayerManager().GetPlayerByController(controller);
            var statistics = playerStatistics[index];

            // set active player flag on certain conditions
            // this is required to allow vanilla end of game statistics to work
            statistics.isActivePlayer = controller.disconnectedMidGame
                                        || controller.isPlayerDead
                                        || controller.isPlayerControlled;

            // skip player if they're not active
            if (!statistics.isActivePlayer) continue;

            players[player] = statistics;
        }

        // create a map of player to player notes
        var playerNotes = new Dictionary<Player.Player, ISet<string>>();
        foreach (var player in players.Keys)
        {
            playerNotes[player] = new HashSet<string>();
        }

        // iterate through all player notes
        foreach (var note in plugin.GetPlayerNoteManager().GetPlayerNotes())
        {
            // sort the players
            var sortedPlayers = players.Keys.ToList();
            sortedPlayers.Sort(note);

            // add to player with the highest index
            var highestPlayer = sortedPlayers.Last();
            playerNotes[highestPlayer].Add(note.GetName());
        }

        // iterate through all players
        foreach (var player in playerNotes.Keys)
        {
            // construct and call pre-event
            var notesAddEvent = new NotesAddEvent.PreEvent(player, playerNotes[player]);
            NotesAddEvent.Pre.Invoke(notesAddEvent);

            // extract notes from event
            var notes = notesAddEvent.GetNotes();

            // add all notes to the player
            foreach (var note in notes)
            {
                players[player].playerNotes.Add(note);
            }

            // construct and call post-event
            var postEvent = new NotesAddEvent.PostEvent(player, notes);
            NotesAddEvent.Post.Invoke(postEvent);
        }

        // todo: include vanilla player notes
        return false; // never calculate vanilla player notes
    }

    [HarmonyPatch("StartGame")]
    [HarmonyPrefix]
    private static void PreStartGame()
    {
        if (RoundStartEvent.Pre == null) return;
        RoundStartEvent.Pre.Invoke();
    }

    [HarmonyPatch("StartGame")]
    [HarmonyPostfix]
    private static void PostStartGame()
    {
        if (RoundStartEvent.Post == null) return;
        RoundStartEvent.Post.Invoke();
    }
}