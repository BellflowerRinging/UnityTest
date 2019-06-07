using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventCenter
{
    // Dictionary<string, UnityEvent<IMessage>> m_EventDic;
    [SerializeField]
    private Dictionary<EventType, UnityEventHandle> m_EventHandelDic;

    public readonly string Name;

    public EventCenter(string name)
    {
        Name = name;
        m_EventHandelDic = new Dictionary<EventType, UnityEventHandle>();
    }

    public void SetListener(SimpleEvent e)
    {
        UnityEventHandle handler;

        if (!m_EventHandelDic.TryGetValue(e.Type, out handler))
        {
            handler = new UnityEventHandle();
            m_EventHandelDic.Add(e.Type, handler);
        }

        handler.SetListener(e);
    }

    public void RemoveListener(SimpleEvent e)
    {
        UnityEventHandle handler;
        if (!m_EventHandelDic.TryGetValue(e.Type, out handler)) return;
        handler.RemoveListener(e);
    }

    public void FireEvent(EventType type, string sender, IMessage message = null)
    {
        if (type == EventType.NULL) return;
        message = message ?? new EmptyMessage();
        Debug.Log(string.Format("[EventCenter]{0} Fire Event:{1} - {2} - {3}", Name, Enum.GetName(typeof(EventType), type), sender, message.ToString()));

        foreach (var item in m_EventHandelDic)
        {
            EventType t = item.Key;
            UnityEventHandle h = item.Value;
            
            if (t == type)
            {
                h.Invoke(sender, message);
                break;
            }
        }

        FireEvent(EventRelation.GetEventParent(type), sender, message);
    }
}