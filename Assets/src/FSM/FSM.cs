using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    IFSMContorl m_ai_fsm;
    void Awake()
    {
        m_ai_fsm = new FSMContorl("simple_ai_fsm_contorl", new AiFSMManager());

        m_ai_fsm.AddListenerToSwitch((mes) =>
        {
            Debug.Log("ListenerToSwitch Last:" + mes.LastStateType.ToString() + " - Cur:" + mes.CurStateType.ToString());
        });

        m_ai_fsm.SwitchTo(typeof(AiMoveState));
    }

    void Update()
    {
        //m_ai_fsm.Running();
    }
}

public class FSMContorl : IFSMContorl
{
    public string m_contorl_name { get; private set; }

    Dictionary<System.Type, IFSMState> m_state_pool;

    IFSMManager m_fsm_manager;

    IFSMState m_cur_state;

    Action<FSMStateSwitchMessage> m_switch_listener;

    public IFSMState Cur_state
    {
        get
        {
            return m_cur_state;
        }

        set
        {
            var mes = new FSMStateSwitchMessage();

            mes.LastStateType = m_cur_state?.GetType();
            mes.CurStateType = value.GetType();

            m_cur_state = value;

            m_switch_listener?.Invoke(mes);
        }
    }

    public FSMContorl(string contorl_name, IFSMManager manager)
    {
        m_contorl_name = contorl_name;

        m_fsm_manager = manager;

        m_state_pool = m_fsm_manager.InitState();

        IFSMState temp;

        if (!m_state_pool.TryGetValue(m_fsm_manager.GetDefaultStateType(), out temp))
        {
            throw new System.Exception("not fount default state");
        }

        Cur_state = temp;

        var mes = new FSMStateMessage();

        mes.LastStateType = null;
        mes.CurStateType = Cur_state.GetType();

        Cur_state.Enter(mes);
    }

    public void SwitchTo(System.Type state)
    {
        IFSMState to_state;

        if (!m_state_pool.TryGetValue(state, out to_state))
        {
            throw new System.Exception("not fount default state");
        }

        var mes = new FSMStateMessage();

        mes.LastStateType = Cur_state.GetType();
        mes.CurStateType = to_state.GetType();

        Cur_state.Exit(mes);

        Cur_state = to_state;

        Cur_state.Enter(mes);
    }

    public void Running()
    {
        Cur_state.Running(new FSMStateMessage());
    }

    public void AddListenerToSwitch(Action<FSMStateSwitchMessage> action)
    {
        m_switch_listener += action;
    }

    public void RemoveListenerToSwitch(Action<FSMStateSwitchMessage> action)
    {
        m_switch_listener -= action;
    }

}