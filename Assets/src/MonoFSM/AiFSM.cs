using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFSM : MonoBehaviour
{
    public PlayerFSM m_player_fsm;
    IMonoFSMContorl<IAiFsmState> m_ai_fsm;
    void Start()
    {
        Dictionary<Type, IAiFsmState> state_pool = new Dictionary<Type, IAiFsmState>();

        state_pool.Add(typeof(AiLaughState), new AiLaughState(this));
        state_pool.Add(typeof(AiMoveState), new AiMoveState(this));

        m_ai_fsm = new MonoFSMContorl<IAiFsmState>(this, "simple_ai_fsm_contorl", state_pool, typeof(AiLaughState));

        m_ai_fsm.AddListenerToSwitch((mes) =>
        {
            Debug.Log("ListenerToSwitch Last:" + mes.LastStateType.ToString() + " - Cur:" + mes.CurStateType.ToString());
        });

        //m_ai_fsm.SwitchTo(typeof(AiMoveState));

        m_player_fsm.AddListenerToSwitch((mes) =>
        {
            if (mes.CurStateType == typeof(PlayerIdleState))
            {
                m_ai_fsm.SwitchTo(typeof(AiLaughState));
            }
            else
            {
                m_ai_fsm.SwitchTo(typeof(AiMoveState));
            }

        });
    }

    void Update()
    {
        if (m_ai_fsm.CurState.GetType() == typeof(AiMoveState))
        {
            m_ai_fsm.Running(new AiMoveState.RunningMessage(m_player_fsm.gameObject));
        }
        else m_ai_fsm.Running(null);
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

        public bool isEmpty()
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