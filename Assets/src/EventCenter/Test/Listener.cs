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
        SimpleEvent prop_change = new SimpleEvent(ActorEventType.PROR_CHANGE, "PlayerA", (message) =>
        {
            var mes = message.ConvertTo<ActorChangeMessage>(false);
            Debug.Log(string.Format("响应事件输出内容 属性：{0}，改变前：{1}，改变后:{2}", mes.PropName, mes.CurValue, mes.ChangeValue));
        });

        m_game.EventCenter.SetListener(prop_change);

        // 2.怪物死亡 发送者可谓null 这就意味着任何怪物死亡都会被监听
        SimpleEvent monster_meath = new SimpleEvent(MonsterEventType.DEATH, null, (message) =>
        {
            var mes = message.ConvertTo<MonsterDeathMessage>(false);
            Debug.Log(string.Format("响应事件输出内容 怪物：{0}，击杀者：{1}", mes.Monster.ToString(), mes.Killer.ToString()));

            // if(mes.Monster == monster && player==self) do..
        });

        m_game.EventCenter.SetListener(monster_meath);
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

    public bool IsEmpty()
    {
        return false; //偷懒不写
    }
}

public struct MonsterDeathMessage : IMessage
{
    public object Monster;     //属性名称
    public object Killer;     //改变后的值

    public MonsterDeathMessage(object monster, object killer)
    {
        Monster = monster;
        Killer = killer;
    }

    public bool IsEmpty()
    {
        return false; //偷懒不写
    }
}