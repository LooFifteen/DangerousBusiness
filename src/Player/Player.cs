using GameNetcodeStuff;
using HarmonyLib;

namespace DangerousBusiness.Player;

public class Player
{
    private readonly PlayerControllerB _controller;

    internal Player(PlayerControllerB controller)
    {
        _controller = controller;
    }

    public string GetUsername()
    {
        return _controller.playerUsername;
    }

    public ulong GetSteamId()
    {
        return _controller.playerSteamId;
    }

    public PlayerControllerB GetController()
    {
        return _controller;
    }

    public override bool Equals(object obj)
    {
        return obj is Player player && _controller.Equals(player._controller);
    }

    public override int GetHashCode()
    {
        return _controller.GetHashCode();
    }
}