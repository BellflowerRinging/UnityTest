using UnityEngine;


public interface IMessage
{
    string ToString();
}
public struct EmptyMessage : IMessage
{
    public bool IsEmpty()
    {
        return true;
    }
}

public struct Message<T> : IMessage
{
    public T t;

    public Message(T t)
    {
        this.t = t;
    }

    public bool IsEmpty()
    {
        return t == null;
    }
}

public struct Message<T, K> : IMessage
{
    public T t;
    public K k;

    public Message(T t, K k)
    {
        this.t = t;
        this.k = k;
    }

    public bool IsEmpty()
    {
        return t == null && k == null;
    }
}

public struct Message<T, K, V> : IMessage
{
    public T t;
    public K k;
    public V v;

    public Message(T t, K k, V v)
    {
        this.t = t;
        this.k = k;
        this.v = v;
    }

    public bool IsEmpty()
    {
        return t == null && k == null && v == null;
    }
}