using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct FSMStateMessage : IMessage
{
    public System.Type LastStateType;
    public System.Type CurStateType;

    public bool IsEmpty()
    {
        return false;
    }
}

public interface IFSMStateSwitchMessage : IMessage
{
    System.Type LastStateType { get; }
    System.Type CurStateType { get; }
}

public struct FSMStateSwitchMessage : IFSMStateSwitchMessage
{
    public System.Type LastStateType { get; set; }
    public System.Type CurStateType { get; set; }

    public bool IsEmpty()
    {
        return false;
    }
}


public interface IFSMStateRunningMessage : IMessage
{
}

public struct FSMStateRunningMessage : IFSMStateRunningMessage
{
    public bool IsEmpty()
    {
        return false;
    }
}