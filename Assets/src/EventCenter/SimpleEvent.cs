using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISimpleEvent
{
    string Sender { get; }
    EventType Type { get; }
    bool IsOnce { get; }
    bool IsEmpty { get; }
    bool IsDeath { get; }
    bool IsActivte { get; }
    void Invoke(IMessage message);

    bool Equals(object obj);
}

public enum EventType
{
    NULL = 2,
    ANY_ONE = 4,
    UI_EVENT = 8,
    UI_OPEN = 16,
    UI_CLOSE = 32,
    CONFIGCHANGE = 64,
    SagaShopEvent = 128,
    ActorPropChange = 256,
}

public static class EventRelation
{
    static Dictionary<EventType, EventType> m_dic;
    static EventRelation()
    {
        m_dic = new Dictionary<EventType, EventType>();
        m_dic.Add(EventType.ANY_ONE, EventType.NULL);

        m_dic.Add(EventType.UI_EVENT, EventType.ANY_ONE);

        m_dic.Add(EventType.UI_OPEN, EventType.UI_EVENT);
        m_dic.Add(EventType.UI_CLOSE, EventType.UI_EVENT);
    }

    public static EventType GetEventParent(EventType type)
    {
        if (m_dic.ContainsKey(type))
            return m_dic[type];
        return EventType.NULL;
    }

}


public class SimpleEvent : ISimpleEvent, IResetAble
{
    EventType type;
    UnityAction<IMessage> Call;
    bool Once = false;
    string sender;
    static readonly public SimplePool<SimpleEvent> S_Pool;

    public string Sender { get { return sender; } }
    public EventType Type { get { return type; } }

    public virtual bool IsEmpty
    {
        get
        {
            return EventType.NULL == type || Call == null || string.IsNullOrEmpty(sender);
        }
    }

    public virtual bool IsDeath
    {
        get
        {
            return IsEmpty;
        }
    }

    public virtual bool IsOnce
    {
        get
        {
            return Once;
        }
    }

    public virtual bool IsActivte
    {
        get
        {
            return true;
        }
    }

    static SimpleEvent()
    {
        S_Pool = new SimplePool<SimpleEvent>();
    }

    public static void Push(SimpleEvent e)
    {
        S_Pool.Free(e);
    }
    public static SimpleEvent Pop(EventType type, string sender, UnityAction<IMessage> call, bool once = false)
    {
        var e = S_Pool.Get();
        e.sender = sender;
        e.type = type;
        e.Call = call;
        e.Once = once;
        return e;
    }

    public SimpleEvent()
    {
        Reset();
    }

    public SimpleEvent(EventType type, string sender, UnityAction<IMessage> call, bool once = false)
    {
        this.sender = sender;
        this.type = type;
        this.Call = call;
        Once = once;
    }
    public void Reset()
    {
        this.sender = null;
        this.type = EventType.NULL;
        this.Call = null;
        Once = false;
    }

    public void Invoke(IMessage message)
    {
        if (IsEmpty || IsDeath) return;
        Call(message);
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        return CheckSimpleEquals(obj);
    }

    /*同一个发送者 同一种类型 只能有一个监听位置  */
    protected bool CheckSimpleEquals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var e = (SimpleEvent)obj;
        return this.Sender == e.Sender
        && this.Type == e.Type;
        // && this.Call == e.Call;  
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return base.GetHashCode();
    }


}

public class MonoEvent : SimpleEvent
{
    readonly MonoBehaviour Mono;

    public override bool IsEmpty
    {
        get
        {
            return base.IsEmpty || Mono == null;
        }
    }

    public override bool IsDeath
    {
        get
        {
            return IsEmpty;
        }
    }

    public override bool IsActivte
    {
        get
        {
            return Mono != null
                && Mono.gameObject.activeInHierarchy
                && Mono.gameObject.activeSelf;
        }
    }

    public MonoEvent(EventType type, string sender, MonoBehaviour mono, UnityAction<IMessage> call, bool once = false) : base(type, sender, call, once)
    {
        this.Mono = mono;
    }

    public override bool Equals(object obj)
    {
        if (!base.CheckSimpleEquals(obj)) return false;

        var e = (MonoEvent)obj;

        return this.Mono == e.Mono
        && base.Equals(obj);
    }

    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return base.GetHashCode();
    }
}
