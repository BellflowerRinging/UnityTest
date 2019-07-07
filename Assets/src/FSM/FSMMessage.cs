using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct FSMStateMessage : IMessage
{
    public System.Type LastStateType;
    public System.Type CurStateType;

    public bool isEmpty()
    {
        return false;
    }
}

public struct FSMStateSwitchMessage : IMessage
{
    public System.Type LastStateType;
    public System.Type CurStateType;

    public bool isEmpty()
    {
        return false;
    }
}