using System.Collections.Generic;

namespace DangerousBusiness.Notes;

public class PlayerNoteManager
{
    private readonly ISet<IPlayerNote> _playerNotes = new HashSet<IPlayerNote>();

    /// <summary>
    /// Add a custom player note.
    /// </summary>
    /// <param name="note">note to add</param>
    public void AddPlayerNote(IPlayerNote note)
    {
        _playerNotes.Add(note);
        DangerousBusiness.GetInstance().GetLogger().LogInfo(_playerNotes.Count + " player notes loaded!");
    }

    /// <summary>
    /// Remove a custom player note.
    /// </summary>
    /// <param name="note">note to remove</param>
    public void RemovePlayerNote(IPlayerNote note)
    {
        _playerNotes.Remove(note);
    }

    /// <summary>
    /// Get an unmodifiable set of all player notes.
    /// </summary>
    /// <returns>an unmodifiable set</returns>
    public ISet<IPlayerNote> GetPlayerNotes()
    {
        return new HashSet<IPlayerNote>(_playerNotes);
    }
}