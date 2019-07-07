using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AiFSMManager : IFSMManager
{
    public Type GetDefaultStateType()
    {
        return typeof(AiLaughState);
    }

    public Dictionary<Type, IFSMState> InitState()
    {
        var dic = new Dictionary<Type, IFSMState>();

        dic.Add(typeof(AiLaughState), new AiLaughState());
        dic.Add(typeof(AiMoveState), new AiMoveState());

        return dic;
    }
}


public class AiLaughState : IAiFsmState
{
    public void Enter(FSMStateMessage message)
    {
        Debug.Log("Enter AiLaughState");
    }

    public void Exit(FSMStateMessage message)
    {
        Debug.Log("Exit AiLaughState");
    }

    public void Running(FSMStateMessage message)
    {
        Debug.Log("Running AiLaughState");
    }
}

public class AiMoveState : IAiFsmState
{
    public void Enter(FSMStateMessage message)
    {
        Debug.Log("Enter AiMoveState");
    }

    public void Exit(FSMStateMessage message)
    {
        Debug.Log("Exit AiMoveState");
    }

    public void Running(FSMStateMessage message)
    {
        Debug.Log("Running AiMoveState");
    }
}