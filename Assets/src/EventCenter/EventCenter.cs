using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventCenter
{
    // Dictionary<string, UnityEvent<IMessage>> m_EventDic;
    [SerializeField]
    private Dictionary<string, UnityEventHandle> m_EventHandelDic;

    public readonly string Name;

    public EventCenter(string name)
    {
        Name = name;
        m_EventHandelDic = new Dictionary<string, UnityEventHandle>();
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

    public enum TransmitType
    {
        NONE,
        TOP_DOWN,
        DOWN_TOP,
    }

    public void FireEvent(string type, string sender, IMessage message = null, bool transmit = true)
    {
        if (type == BaseEventType.NULL) return;

        var e = sender == null ? type : string.Format("{0}.{1}", type, sender);

        var event_list = e.Split('.');

        message = message ?? new EmptyMessage();

        Debug.Log(string.Format("[EventCenter]{0} Fire Event:{1} - {2} - {3}", Name, type, sender, message.ToString()));

        System.Text.StringBuilder event_type = new System.Text.StringBuilder();

        for (int i = 0; i < event_list.Length; i++)
        {
            event_type.Append(event_list[i]);

            foreach (var item in m_EventHandelDic)
            {
                string t = item.Key;
                UnityEventHandle h = item.Value;

                if (t == event_type.ToString())
                {
                    h.Invoke(sender, message);
                    break;
                }
            }

            event_type.Append(".");
        }

    }
}