using System;
using System.Collections.Generic;

[Serializable]
public class UnityEventHandle
{
    List<ISimpleEvent> m_events;
    public UnityEventHandle()
    {
        m_events = new List<ISimpleEvent>();
    }

    public void SetListener(ISimpleEvent e)
    {
        if (e == null || e.IsEmpty) return;
        if (m_events.Contains(e)) return;
        m_events.Add(e);
    }

    public void RemoveListener(ISimpleEvent e)
    {
        m_events.Remove(e);
    }

    public void Invoke(string sender, IMessage message)
    {
        List<ISimpleEvent> to_remove = new List<ISimpleEvent>();
        foreach (var e in m_events)
        {
            if (e.Sender != sender) continue;

            if (e.IsDeath)
            {
                to_remove.Add(e);
                continue;
            }

            if (!e.IsActivte) continue;

            e.Invoke(message);

            if (e.IsOnce) to_remove.Add(e);
        }

        foreach (var e in to_remove)
        {
            m_events.Remove(e);

        }
    }
}