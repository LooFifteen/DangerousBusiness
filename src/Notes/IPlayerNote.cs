using System.Collections.Generic;
using GameNetcodeStuff;

namespace DangerousBusiness.Notes;

public interface IPlayerNote : IComparer<PlayerControllerB>
{
    /// <summary>
    /// Display name of the note, will be shown under "Notes:"
    /// </summary>
    /// <returns>Display name</returns>
    public string GetName();
}