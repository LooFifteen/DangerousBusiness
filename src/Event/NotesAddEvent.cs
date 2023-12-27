using System.Collections.Generic;
using UnityEngine.Events;

namespace DangerousBusiness.Event;

public abstract class NotesAddEvent
{
    public static readonly UnityEvent<PreEvent> Pre = new ();
    public static readonly UnityEvent<PostEvent> Post = new ();

    private NotesAddEvent()
    {

    }

    public class PreEvent : IPlayerEvent
    {
        private readonly Player.Player _player;
        private readonly ISet<string> _notes;

        internal PreEvent(Player.Player player, IEnumerable<string> notes)
        {
            _player = player;
            _notes = new HashSet<string>(notes);
        }

        public Player.Player GetPlayer()
        {
            return _player;
        }

        public ISet<string> GetNotes()
        {
            return _notes;
        }
    }

    public class PostEvent : IPlayerEvent
    {
        private readonly Player.Player _player;
        private readonly ISet<string> _notes;

        internal PostEvent(Player.Player player, ISet<string> notes)
        {
            _player = player;
            _notes = notes;
        }

        public Player.Player GetPlayer()
        {
            return _player;
        }

        public IEnumerable<string> GetNotes()
        {
            return new HashSet<string>(_notes);
        }
    }
}