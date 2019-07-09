using System;
using UnityEngine;


public class PlayerContorl : MonoBehaviour
{
    public float m_speed = 10f;

    PlayerFSM m_player_fsm;

    void Start()
    {
        m_player_fsm = GetComponent<PlayerFSM>();
    }

    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(m_speed * transform.forward * Time.deltaTime, Space.Self);
            m_player_fsm.SwitchTo(typeof(PlayerMoveState));
        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(m_speed * transform.forward * Time.deltaTime * -1, Space.Self);
            m_player_fsm.SwitchTo(typeof(PlayerMoveState));
        }
        else if (Input.GetKey("a"))
        {
            transform.Translate(m_speed * (Quaternion.Euler(0, -90, 0) * transform.forward) * Time.deltaTime, Space.Self);
            m_player_fsm.SwitchTo(typeof(PlayerMoveState));
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(m_speed * (Quaternion.Euler(0, 90, 0) * transform.forward) * Time.deltaTime, Space.Self);
            m_player_fsm.SwitchTo(typeof(PlayerMoveState));
        }
        else
        {
            m_player_fsm.SwitchTo(typeof(PlayerIdleState));
        }
    }



}

