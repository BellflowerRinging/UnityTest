using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFSMState
{
    void Enter(FSMStateMessage message);
    void Running(FSMStateMessage message);
    void Exit(FSMStateMessage message);
}

public interface IAiFsmState : IFSMState
{

}

public interface IFSMManager
{
    Dictionary<System.Type, IFSMState> InitState();
    System.Type GetDefaultStateType();
}

public interface IFSMContorl
{
    IFSMState Cur_state { get; set; }

    void AddListenerToSwitch(Action<FSMStateSwitchMessage> action);

    void RemoveListenerToSwitch(Action<FSMStateSwitchMessage> action);

    void SwitchTo(System.Type state);

    void Running();
}