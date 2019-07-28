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
        var forward = Vector3.zero;
        if (Input.GetKey("w"))
        {
            forward = transform.forward;
        }
        else if (Input.GetKey("s"))
        {
            forward = transform.forward * -1;
        }
        else if (Input.GetKey("a"))
        {
            forward = Quaternion.Euler(0, -90, 0) * transform.forward;
        }
        else if (Input.GetKey("d"))
        {
            forward = Quaternion.Euler(0, 90, 0) * transform.forward;
        }

        if (forward != Vector3.zero)
        {
            transform.Translate(m_speed * forward * Time.deltaTime, Space.Self);
            m_player_fsm.SwitchTo(typeof(PlayerMoveState));
        }
        else
        {
            m_player_fsm.SwitchTo(typeof(PlayerIdleState));
        }
    }
}

