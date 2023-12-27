using BepInEx.Logging;
using DangerousBusiness.Event;
using DangerousBusiness.Note;
using DangerousBusiness.Player;

namespace DangerousBusiness;

using BepInEx;
using HarmonyLib;

[BepInPlugin(PluginGuid, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class DangerousBusiness : BaseUnityPlugin
{
    private const string PluginGuid = "dev.lu15.dangerousbusiness";

    private static DangerousBusiness _plugin;

    private readonly Harmony _harmony = new (PluginGuid);
    private readonly PlayerManager _playerManager = new ();
    private readonly NoteManager _noteManager = new ();

    private void Awake()
    {
        // set the static instance
        _plugin = this;

        // todo: tests
        _noteManager.AddPlayerNote(new HealthiestPlayerNote());
        RoundStartEvent.Post.AddListener(() =>
        {
            HUDManager.Instance.AddTextToChatOnServer("The round has started!");
        });
        NotesAddEvent.Pre.AddListener(e =>
        {
            e.GetNotes().Add("absolute loser");
        });

        // patch the game
        _harmony.PatchAll();

        // log that we're loaded
        Logger.LogInfo("DangerousBusiness loaded!");
    }

    /// <summary>
    /// The primary instance of the DangerousBusiness plugin.
    /// </summary>
    /// <returns>DangerousBusiness instance</returns>
    public static DangerousBusiness GetInstance()
    {
        return _plugin;
    }

    /// <summary>
    /// The DangerousBusiness internal logger. Not to be used by dependent plugins.
    /// </summary>
    /// <returns>manual log source</returns>
    public ManualLogSource GetLogger()
    {
        return Logger;
    }

    /// <summary>
    /// The player manager.
    /// </summary>
    /// <returns></returns>
    public PlayerManager GetPlayerManager()
    {
        return _playerManager;
    }

    /// <summary>
    /// The custom player note manager.
    /// </summary>
    /// <returns>note manager</returns>
    public NoteManager GetPlayerNoteManager()
    {
        return _noteManager;
    }

    private class HealthiestPlayerNote : INote
    {
        public string GetName()
        {
            return "Healthiest Player";
        }

        public int Compare(Player.Player x, Player.Player y)
        {
            if (x == null || y == null) return 0;
            return x.GetController().health.CompareTo(y.GetController().health);
        }
    }
}