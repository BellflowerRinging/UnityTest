using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour
{

    public Game m_game;

    void Start()
    {

        ActorChangeMessage mes = new ActorChangeMessage("Hp", "100", "50");
        m_game.EventCenter.FireEvent(EventType.ActorPropChange, "一个不应该被知道的发送者", mes);

    }

}
