using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class UnityEventFunction
{
    public static void SetListener(this UnityEvent e, UnityAction action)
    {
        e.RemoveAllListeners();
        e.AddListener(action);
    }

    public static void SetListener(this UnityEvent<IMessage> e, UnityAction<IMessage> action)
    {
        e.RemoveAllListeners();
        e.AddListener(action);
    }

    public static void AddListener(this UnityEvent<IMessage> e, UnityAction<IMessage> action)
    {
        e.RemoveAllListeners();
        e.AddListener(action);
    }

    public static void Set<T, V>(this Dictionary<T, V> d, T key, V value)
    {
        if (d.ContainsKey(key)) d[key] = value;
        else d.Add(key, value);
    }


    /// 如果死必须的类型不对抛出异常 如果不是必须的返回Defaule（T）
    public static T ConvertTo<T>(this IMessage m, bool MustBe = true) where T : IMessage
    {
        if (m.GetType() != typeof(T))
        {
            if (MustBe) throw new UnityException("ConvertTo Error m type:" + m.GetType() + " To Type:" + typeof(T));
            return default(T);
        }
        return (T)m;
    }

    public static T GetComponentInArrayByName<T>(this Component[] array,string name) where T : Component
    {
        foreach (var item in array)
        {
            if (item.name == name)
                return (T)item;
        }

        return null;
    }
}
