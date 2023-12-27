using System.Collections.Generic;

namespace DangerousBusiness.Note;

public class NoteManager
{
    private readonly ISet<INote> _playerNotes = new HashSet<INote>();

    internal NoteManager()
    {

    }

    /// <summary>
    /// Add a custom player note.
    /// </summary>
    /// <param name="note">note to add</param>
    public void AddPlayerNote(INote note)
    {
        _playerNotes.Add(note);
        DangerousBusiness.GetInstance().GetLogger().LogInfo(_playerNotes.Count + " player notes loaded!");
    }

    /// <summary>
    /// Remove a custom player note.
    /// </summary>
    /// <param name="note">note to remove</param>
    public void RemovePlayerNote(INote note)
    {
        _playerNotes.Remove(note);
    }

    /// <summary>
    /// Get an unmodifiable set of all player notes.
    /// </summary>
    /// <returns>an unmodifiable set</returns>
    public ISet<INote> GetPlayerNotes()
    {
        return new HashSet<INote>(_playerNotes);
    }
}