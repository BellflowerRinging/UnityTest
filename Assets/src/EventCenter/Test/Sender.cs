using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour
{

    public Game m_game;

    void Start()
    {

        ActorChangeMessage mes = new ActorChangeMessage("Hp", "100", "50");
        m_game.EventCenter.FireEvent(ActorEventType.PROR_CHANGE, "PlayerA", mes);

        MonsterDeathMessage mes1 = new MonsterDeathMessage("MonsterA", "PlayerA");
        m_game.EventCenter.FireEvent(MonsterEventType.DEATH, null, mes1);
    }

}
