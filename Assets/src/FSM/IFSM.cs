using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IFSMState
{
    void Enter(FSMStateMessage message);
    void Running(FSMStateMessage message);
    void Exit(FSMStateMessage message);
}

public interface IFSMManager
{
    Dictionary<System.Type, IFSMState> InitState();
    System.Type GetDefaultStateType();
}

public interface IFSMContorl
{
    IFSMState Cur_state { get; set; }

    void AddListenerToSwitch(Action<IFSMStateSwitchMessage> action);

    void RemoveListenerToSwitch(Action<IFSMStateSwitchMessage> action);

    void SwitchTo(System.Type state);

    void Running();
}

public class FSMContorl : IFSMContorl
{
    public string m_contorl_name { get; private set; }

    Dictionary<System.Type, IFSMState> m_state_pool;

    IFSMManager m_fsm_manager;

    IFSMState m_cur_state;

    Action<IFSMStateSwitchMessage> m_switch_listener;

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

        var default_type = m_fsm_manager.GetDefaultStateType();

        if (!m_state_pool.TryGetValue(default_type, out temp))
        {
            throw new System.Exception("'" + m_contorl_name + "' not fount '" + default_type.Name + "' default state");
        }

        Cur_state = temp;

        var mes = new FSMStateMessage();

        mes.LastStateType = null;
        mes.CurStateType = Cur_state.GetType();

        Cur_state.Enter(mes);
    }

    public void SwitchTo(System.Type state)
    {
        if (m_cur_state.GetType() == state) return;

        IFSMState to_state;

        if (!m_state_pool.TryGetValue(state, out to_state))
        {
            throw new System.Exception("'" + m_contorl_name + "' not fount '" + state.Name + "' state");
        }

        var mes = new FSMStateMessage();

        mes.LastStateType = Cur_state.GetType();
        mes.CurStateType = to_state.GetType();

        Cur_state.Exit(mes);

        Cur_state.Enter(mes);

        Cur_state = to_state;
    }

    public void Running()
    {
        Cur_state.Running(new FSMStateMessage());
    }

    public void AddListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_switch_listener += action;
    }

    public void RemoveListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_switch_listener -= action;
    }

}
