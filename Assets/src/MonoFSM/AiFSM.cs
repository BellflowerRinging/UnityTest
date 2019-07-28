using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFSM : MonoBehaviour
{
    public PlayerFSM m_player_fsm; //由Unity拖进来
    IMonoFSMContorl<IAiFsmState> m_fsm_ctrl;
    void Start()
    {
        Dictionary<Type, IAiFsmState> state_pool = new Dictionary<Type, IAiFsmState>();

        //手动初始化状态池，这一步是不是可以通过什么途径通用化？存疑
        state_pool.Add(typeof(AiLaughState), new AiLaughState(this));
        state_pool.Add(typeof(AiMoveState), new AiMoveState(this));

        m_fsm_ctrl = new MonoFSMContorl<IAiFsmState>(this, "simple_ai_fsm_contorl", state_pool, typeof(AiLaughState));

        //测试监听器
        m_fsm_ctrl.AddListenerToSwitch((mes) =>
        {
            Debug.Log("ListenerToSwitch Last:" + mes.LastStateType.ToString() + " - Cur:" + mes.CurStateType.ToString());
        });

        //监听玩家状态改变
        m_player_fsm.AddListenerToSwitch((mes) =>
        {
            if (mes.CurStateType == typeof(PlayerIdleState)) //根据玩家当前状态切换自身状态
            {
                m_fsm_ctrl.SwitchTo(typeof(AiLaughState));
            }
            else
            {
                m_fsm_ctrl.SwitchTo(typeof(AiMoveState));
            }
        });
    }

    void Update() //调用Running方法，实现状态的运行
    {
        if (m_fsm_ctrl.CurState.GetType() == typeof(AiMoveState))//如果Ai处于移动状态就传入玩家数据
        {
            m_fsm_ctrl.Running(new AiMoveState.RunningMessage(m_player_fsm.gameObject));
        }
        else m_fsm_ctrl.Running(null);
    }
}

public abstract class IAiFsmState : MonoFSMState
{
    public IAiFsmState(MonoBehaviour mono) : base(mono)
    {
    }
}



public class AiLaughState : IAiFsmState
{
    public AiLaughState(MonoBehaviour mono) : base(mono)
    {
    }

    public override void Enter(IFSMStateSwitchMessage message)
    {
        Debug.Log("Enter AiLaughState");
    }

    public override void Exit(IFSMStateSwitchMessage message)
    {
        Debug.Log("Exit AiLaughState");
    }

    public override void Running(IFSMStateRunningMessage message)
    {
        Mono.gameObject.transform.Rotate(new Vector3(0, 400f * Time.deltaTime, 0));
    }
}

public class AiMoveState : IAiFsmState
{
    public struct RunningMessage : IFSMStateRunningMessage
    {
        public GameObject Player;

        public RunningMessage(GameObject player)
        {
            Player = player;
        }

        public bool IsEmpty()
        {
            return Player == null;
        }
    }

    public AiMoveState(MonoBehaviour mono) : base(mono)
    {

    }

    public override void Enter(IFSMStateSwitchMessage message)
    {
        Debug.Log("Enter AiMoveState");
    }

    public override void Exit(IFSMStateSwitchMessage message)
    {
        Debug.Log("Exit AiMoveState");
    }

    public override void Running(IFSMStateRunningMessage message)
    {
        var player = message.ConvertTo<RunningMessage>().Player;

        var dis = player.transform.position - Mono.transform.position;
        var forward = dis.normalized;
        var man = (int)dis.magnitude;

        if (man < 5)
        {
            forward *= -1;
        }
        else if (man == 5)
        {
            forward = Vector3.zero;
            Mono.gameObject.transform.Rotate(new Vector3(0, 400f * Time.deltaTime, 0));
        }


        Mono.transform.Translate(forward * Time.deltaTime * 10f, Space.World);
        //Mono.gameObject.transform.Rotate(new Vector3(0, 5f * Time.deltaTime, 0));
    }
}