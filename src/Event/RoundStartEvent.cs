using UnityEngine.Events;

namespace DangerousBusiness.Event;

public abstract class RoundStartEvent
{
    public static readonly UnityEvent Pre = new ();
    public static readonly UnityEvent Post = new ();

    private RoundStartEvent()
    {

    }
}