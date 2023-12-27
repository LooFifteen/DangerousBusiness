using BepInEx.Logging;
using DangerousBusiness.Notes;
using GameNetcodeStuff;

namespace DangerousBusiness;

using BepInEx;
using HarmonyLib;

[BepInPlugin(PluginGuid, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class DangerousBusiness : BaseUnityPlugin
{
    private const string PluginGuid = "dev.lu15.dangerousbusiness";

    private static DangerousBusiness _plugin;

    private readonly Harmony _harmony = new (PluginGuid);
    private readonly PlayerNoteManager _playerNoteManager = new ();

    private void Awake()
    {
        // set the static instance
        _plugin = this;

        // todo: test player notes
        _playerNoteManager.AddPlayerNote(new HealthiestPlayerNote());

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
    /// The custom player note manager.
    /// </summary>
    /// <returns>note manager</returns>
    public PlayerNoteManager GetPlayerNoteManager()
    {
        return _playerNoteManager;
    }

    private class HealthiestPlayerNote : IPlayerNote
    {
        public string GetName()
        {
            return "Healthiest Player";
        }

        public int Compare(PlayerControllerB x, PlayerControllerB y)
        {
            if (x == null || y == null) return 0;
            return x.health.CompareTo(y.health);
        }
    }
}