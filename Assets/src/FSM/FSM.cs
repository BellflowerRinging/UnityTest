using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    IFSMContorl m_ai_fsm;
    void Awake()
    {
        /**
        m_ai_fsm = new FSMContorl("simple_ai_fsm_contorl", new AiFSMManager());

        m_ai_fsm.AddListenerToSwitch((mes) =>
        {
            Debug.Log("ListenerToSwitch Last:" + mes.LastStateType.ToString() + " - Cur:" + mes.CurStateType.ToString());
        });

        m_ai_fsm.SwitchTo(typeof(AiMoveState));*/
    }

    void Update()
    {
        //m_ai_fsm.Running();
    }
}

