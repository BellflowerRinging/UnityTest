using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    IMonoFSMContorl<IPlayerFsmState> m_player_fsm;
    void Awake()
    {
        Dictionary<Type, IPlayerFsmState> state_pool = new Dictionary<Type, IPlayerFsmState>();

        state_pool.Add(typeof(PlayerIdleState), new PlayerIdleState(this));
        state_pool.Add(typeof(PlayerMoveState), new PlayerMoveState(this));

        m_player_fsm = new MonoFSMContorl<IPlayerFsmState>(this, "simple_player_fsm_contorl", state_pool, typeof(PlayerIdleState));

        m_player_fsm.AddListenerToSwitch((mes) =>
        {
            Debug.Log("ListenerToSwitch Last:" + mes.LastStateType.ToString() + " - Cur:" + mes.CurStateType.ToString());
        });
    }

    public void AddListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_player_fsm.AddListenerToSwitch(action);
    }

    public void RemoveListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_player_fsm.RemoveListenerToSwitch(action);
    }

    public void SwitchTo(Type state)
    {
        m_player_fsm.SwitchTo(state);
    }

    void Update()
    {
        m_player_fsm.Running(default(IFSMStateRunningMessage));
    }
}

public abstract class IPlayerFsmState : MonoFSMState
{
    public IPlayerFsmState(MonoBehaviour mono) : base(mono)
    {
    }
}

public class PlayerMoveState : IPlayerFsmState
{
    public PlayerMoveState(MonoBehaviour mono) : base(mono)
    {
    }

    public override void Enter(IFSMStateSwitchMessage message)
    {
        //  Debug.Log("Enter PlayerRunState");
    }

    public override void Exit(IFSMStateSwitchMessage message)
    {
        // Debug.Log("Exit PlayerRunState");
    }

    public override void Running(IFSMStateRunningMessage message)
    {
        // Debug.Log("Running PlayerRunState");
    }
}

public class PlayerIdleState : IPlayerFsmState
{
    public PlayerIdleState(MonoBehaviour mono) : base(mono)
    {
    }

    public override void Enter(IFSMStateSwitchMessage message)
    {
        // Debug.Log("Enter PlayerIdleState");
    }

    public override void Exit(IFSMStateSwitchMessage message)
    {
        // Debug.Log("Exit PlayerIdleState");
    }

    public override void Running(IFSMStateRunningMessage message)
    {
        // Debug.Log("Running PlayerIdleState");
    }
}