using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISimpleEvent
{
    string Sender { get; }
    string Type { get; }
    bool IsOnce { get; }
    bool IsEmpty { get; }
    bool IsDeath { get; }
    bool IsActivte { get; }
    void Invoke(IMessage message);

    bool Equals(object obj);
}

public class BaseEventType
{
    public const string NULL = "NULL";
    public const string ANY_ONE = "ANY_ONE";
    public const string UI_EVENT = "UI_EVENT";
    public const string ACTOR_EVENT = "ACTOR_EVENT";
    public const string MONSTER_EVENT = "MONSTER_EVENT";
}

public class UiEventType
{
    public const string PARENT = BaseEventType.UI_EVENT;
    public const string UI_OPEN = PARENT + ".UI_OPEN";
    public const string UI_CLOSE = PARENT + ".UI_OPEN";
}

public class ActorEventType
{
    public const string PARENT = BaseEventType.ACTOR_EVENT;
    public const string PROR_CHANGE = PARENT + ".PROR_CHANGE";
}

public class MonsterEventType
{
    public const string PARENT = BaseEventType.MONSTER_EVENT;
    public const string DEATH = PARENT + ".DEATH";
}

public class SimpleEvent : ISimpleEvent, IResetAble
{
    UnityAction<IMessage> Call;
    bool Once = false;
    string sender;
    string type;
    static readonly public SimplePool<SimpleEvent> S_Pool;

    public string Sender { get { return sender; } }
    public string Type { get { return type; } }

    public virtual bool IsEmpty
    {
        get
        {
            return BaseEventType.NULL == type || Call == null;
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
    public static SimpleEvent Pop(string type, string sender, UnityAction<IMessage> call, bool once = false)
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

    public SimpleEvent(string type, string sender, UnityAction<IMessage> call, bool once = false)
    {
        this.sender = sender;
        this.type = type;
        this.Call = call;
        Once = once;
    }
    public void Reset()
    {
        this.sender = null;
        this.type = BaseEventType.NULL;
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

    public MonoEvent(string type, string sender, MonoBehaviour mono, UnityAction<IMessage> call, bool once = false) : base(type, sender, call, once)
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
