using System.Collections.Generic;

namespace DangerousBusiness.Note;

public interface INote : IComparer<Player.Player>
{
    /// <summary>
    /// Display name of the note, will be shown under "Notes:"
    /// </summary>
    /// <returns>Display name</returns>
    public string GetName();
}