using System;
using System.Collections.Generic;

public class EventSystem
{
    private List<Event> events = new();
    public IReadOnlyList<Event> Events => events;

    public void Initialize()
    {
        events.Clear();

        var allClasses = Util.Reflection.FindAllSubclasses<Event>();
        foreach (var t in allClasses) if (Activator.CreateInstance(t) is Event instance) events.Add(instance);
    }

    public List<T> FindAll<T>()
    {
        return EventCache<T>.FindAll(this);
    }
}

public static class EventCache<T>
{
    private static List<T> events;

    public static List<T> FindAll(EventSystem eventSystem)
    {
        if (events != null)
            return events;

        events = new List<T>(64);

        foreach (var a in eventSystem.Events)
        {
            if (a is T ast)
                events.Add(ast);
        }

        events.Sort((a, b) => (a as Event).Priority - (b as Event).Priority);

        return events;
    }
}
public static class PriorityLayer
{
    public const int FIRST = -10000;
    public const int NORMAL = 0;
    public const int LAST = 10000;
    public const int LAST_SPECIAL = 10001;
}

public abstract class Event
{
    public virtual int Priority => PriorityLayer.NORMAL;
}