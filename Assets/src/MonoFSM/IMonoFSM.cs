using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class MonoFSMState
{
    protected MonoBehaviour Mono { get; set; }

    public MonoFSMState(MonoBehaviour mono)
    {
        Mono = mono;
    }

    public abstract void Enter(IFSMStateSwitchMessage message);

    public abstract void Exit(IFSMStateSwitchMessage message);

    public abstract void Running(IFSMStateRunningMessage message);
}

public abstract class IMonoFSMContorl<T> where T : MonoFSMState
{
    public abstract string Name { get; protected set; }

    public abstract MonoBehaviour CurMono { get; protected set; }

    public abstract T CurState { get; protected set; }

    protected abstract Dictionary<Type, T> m_state_pool { get; set; }

    protected abstract Action<IFSMStateSwitchMessage> m_switch_listener { get; set; }

    public IMonoFSMContorl(MonoBehaviour mono, string contorl_name, Dictionary<Type, T> state_pool, Type default_type) { }

    public abstract void AddListenerToSwitch(Action<IFSMStateSwitchMessage> action);

    public abstract void RemoveListenerToSwitch(Action<IFSMStateSwitchMessage> action);

    public abstract void SwitchTo(Type state);

    public abstract void Running(IFSMStateRunningMessage message);
}

public class MonoFSMContorl<T> : IMonoFSMContorl<T> where T : MonoFSMState
{
    public override string Name { get; protected set; }

    protected override Dictionary<System.Type, T> m_state_pool { get; set; }

    protected override Action<IFSMStateSwitchMessage> m_switch_listener { get; set; }

    private T m_cur_state;

    public override T CurState
    {
        get
        {
            return m_cur_state;
        }

        protected set
        {
            var mes = new FSMStateSwitchMessage();

            mes.LastStateType = m_cur_state == null ? null : m_cur_state.GetType();
            mes.CurStateType = value.GetType();

            m_cur_state = value;

            if (m_switch_listener != null) m_switch_listener.Invoke(mes);
        }
    }

    public override MonoBehaviour CurMono { get; protected set; }

    public MonoFSMContorl(MonoBehaviour mono, string contorl_name, Dictionary<Type, T> state_pool, Type default_type) : base(mono, contorl_name, state_pool, default_type)
    {
        Name = contorl_name;

        m_state_pool = state_pool;

        T temp;

        if (!m_state_pool.TryGetValue(default_type, out temp))
        {
            throw new System.Exception("'" + Name + "' not fount '" + default_type.Name + "' default state");
        }

        CurState = temp;

        var mes = new FSMStateSwitchMessage();

        mes.LastStateType = null;

        mes.CurStateType = CurState.GetType();

        CurState.Enter(mes);
    }

    public override void SwitchTo(System.Type state)
    {
        if (CurState.GetType() == state) return;

        T to_state;

        if (!m_state_pool.TryGetValue(state, out to_state))
        {
            throw new System.Exception("'" + Name + "' not fount '" + state.Name + "' state");
        }

        var mes = new FSMStateSwitchMessage();

        mes.LastStateType = CurState.GetType();
        mes.CurStateType = to_state.GetType();

        CurState.Exit(mes);

        CurState.Enter(mes);

        CurState = to_state;
    }

    public override void Running(IFSMStateRunningMessage message)
    {
        CurState.Running(message);
    }

    public override void AddListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_switch_listener += action;
    }

    public override void RemoveListenerToSwitch(Action<IFSMStateSwitchMessage> action)
    {
        m_switch_listener -= action;
    }

}