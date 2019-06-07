using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour
{

    public Game m_game;



    void Awake()
    {

        // 1.角色属性改变
        SimpleEvent prop_change = new SimpleEvent(EventType.ActorPropChange, "一个不应该被知道的发送者", (message) =>
        {
            var mes = message.ConvertTo<ActorChangeMessage>(false);
            Debug.Log(string.Format("响应事件输出内容 属性：{0}，改变前：{1}，改变后:{2}", mes.PropName, mes.CurValue, mes.ChangeValue));
        });

        m_game.EventCenter.SetListener(prop_change);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
public struct ActorChangeMessage : IMessage
{
    public string PropName;     //属性名称
    public object CurValue;     //改变后的值
    public object ChangeValue;  //改变了多少

    public ActorChangeMessage(string propName, object curValue, object changeValue)
    {
        PropName = propName;
        CurValue = curValue;
        ChangeValue = changeValue;
    }

    public bool isEmpty()
    {
        return false;
    }
}