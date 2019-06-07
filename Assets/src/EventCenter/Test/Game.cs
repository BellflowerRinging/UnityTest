using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    private EventCenter m_eventCenter;

    public EventCenter EventCenter
    {
        get
        {
            if (m_eventCenter == null) m_eventCenter = new EventCenter("Main Game");
            return m_eventCenter;
        }
    }

    /* 应用场景
     * 1.角色属性改变
     * 2.任务监听怪物死亡
     * 3.新手引导，当UI界面加载完毕时，在相应的位置显示箭头，需要监听UI界面打开完成的事件
    */

    /*
     *1.EventCenter的功能是订阅发布模式的调度中心,我称之为事件中心，是告知所有订阅者某个事件发生的模块。
     * 提供了相应的几个方法
     *  SetListener(SimpleEvent)
     *  RemoveListener(SimpleEvent)
     *  FireEvent(EventType, Sender, IMessage)  事件类型，发送者，事件内容
     *  
     * 并非单例对象
     *  任何核心模块都可以new EventCenter()来管理这个核心模块下的事件调度
     * 
     *2.SimpleEvent最简单的一个事件，是订阅者订阅的事件
     * 构造函数
     *  SimpleEvent(EventType , Sender, UnityAction<IMessage>) 事件类型，发送者，事件发生后要执行的委托
     *  
     *3.例子：PlayerA的生命值改变(EventType.HpChange)更新生命条(HpBar)
     * 
     * HpBar控制类初始化的时候 
     *  SimpleEvent e = new SimpleEvent(EventType.HpChange,"PlayerA",(message)=>{ UpdateHpBar(message); });
     *  Player.EventCenter.SetListener(e);
     *  
     * PlayerA控制类的SetHp方法里
     *  Player.EventCenter.FireEvent(EventType.HpChange, "PlayerA", IMessage);
     * 
     * FireEvent的时候事件中心对所有的订阅者进行遍历当EventType和Sender都符合的时候，调用SimpleEvent.UnityAction<IMessage>委托，并传递message
     * 至此完成一次事件的调度对HpBar和PlayerA进行了双向的解耦
     * 并非完全解耦，HpBar对Sender产生了依赖，虽然是String，不关乎实例对象，但没有做到思想上的解耦，就不算是完全解耦。
     * 
     * 要完全解耦就不能有Sender这个形参
     * FireEvent(EventType, Sender, IMessage) 就会变成 FireEvent(EventType, IMessage)
     * EventType.HpChange可能是PlayerABCD发送的
     * 所以EventType应该因细节到EventType.PlayerA_HpChange,EventType.PlayerB_HpChange,EventType.PlayerC_HpChange... 太多了应该有更好的方案
     */

    /*
     *调度中心是关于订阅者和发布者双向解耦的方案，所以双方都应只关心事件而不去关心谁发送的或是谁接收的，所以事件的结构里应没有接收者和发送者。
     *但对于应用场景第3点
     * 发布者发布的事件应该是“某UI打开完毕”，而不是“UI打开完毕”，发布者是某UI。
     * 订阅者只关心事件本身而不关心发布者
     * 
     *所以事件的构造函数不应该是 
     * SimpleEvent(EventType,Sender,Callback)
     *应该是
     * SimpleEvent(EventType,Callback)
     * 
     *
     * 
     * 
     */


    /* EventType 用枚Enum还是String？
     * 用String表示没有限制并不能规范化，开发人员对属性的命名可能不统一
     * 比如角色生命值属性改变
     *  可能是FireEven("HpChange") 或 FireEven("HealthChange")
     *  若发布者和接受者都是同一个开发人员，测试自己写的HpChange功能可能没有问题，可并不响应对别人写的HealthChange
     * 但没有什么比String所能包含的东西更多，它可以直接描述事件所属关系，比如"Player.PropChange.HpChange"
     * 
     * 用Enum一定程度能避免命名不统一的问题，但也会出现“好像没有这个Type，我新命名一个吧”的情况。
     * 对于事件所属关系的描述能力几乎为0。
     * 
     * 什么方案都会出现人为失误，但越能减少失误的方案越好。
     * 
     */
}


