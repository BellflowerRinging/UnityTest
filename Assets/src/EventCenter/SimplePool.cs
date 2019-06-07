using System.Collections.Generic;
using UnityEngine;

public interface IResetAble
{
    void Reset();
}

public class SimplePool<T> where T : IResetAble, new()
{
    Stack<T> pool;
    public readonly int MaxCount;
    int curCount = 0;
    public SimplePool(int init = 5, int max = 20)
    {
        if (init > max) throw new UnityException("init > max");
        MaxCount = max;
        pool = new Stack<T>();
        for (int i = 0; i < init; i++)
        {
            Add();
        }
    }

    public T Get()
    {
        if (pool.Count == 0) Add();
        return pool.Pop();
    }

    public void Free(T t)
    {
        t.Reset();
        pool.Push(t);
    }

    public void Add()
    {
        pool.Push(new T());
        curCount++;
        if (curCount >= MaxCount) throw new System.Exception("curCount >= MaxCount");
    }
}