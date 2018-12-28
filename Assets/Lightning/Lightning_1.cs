using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_1 : MonoBehaviour
{

    // Use this for initialization
    public Transform m_Star;
    public Transform m_End;
    public LineRenderer m_Line;
    public float MaxSegmentLength;
    [Range(0, 1)]
    public float NoieSpeed = 0.05f;

    public Texture2D[] m_Texture2D;

    Material material;
    Vector3[] NodePostions;
    void Start()
    {
        material = m_Line.material;
    }

    float loctime = 0f;
    // Update is called once per frame
    void Update()
    {
        if (loctime >= NoieSpeed)
        {
            InitLine(m_Line, m_Star.position, m_End.position, MaxSegmentLength);
            loctime = 0;
        }
        else
        {
            loctime += Time.deltaTime;
        }
    }

    void InitLine(LineRenderer line, Vector3 star, Vector3 end, float msLength)
    {
        if (msLength <= 0) return;

        if (m_Texture2D.Length != 0)
            material.SetTexture("_MainTex", m_Texture2D[UnityEngine.Random.Range(0, m_Texture2D.Length)]);


        List<Vector3> list = new List<Vector3>();
        list.Add(star);
        Vector3 loc = star, dirct;

        msLength = UnityEngine.Random.Range(msLength * 0.5f, msLength * 3f);

        while (Vector3.Distance(loc, end) >= msLength)
        {
            dirct = (end - loc).normalized * msLength;
            loc += dirct;
            loc += RandomOffect(msLength * 2 / 3);
            list.Add(loc);
        }

        list.Add(end);
        Vector3[] postions = list.ToArray();
        line.positionCount = postions.Length;
        line.SetPositions(postions);
        return;
    }

    Vector3 RandomOffect(float maxOffset)
    {
        return new Vector3(UnityEngine.Random.Range(-maxOffset, maxOffset), UnityEngine.Random.Range(-maxOffset, maxOffset), UnityEngine.Random.Range(-maxOffset, maxOffset));
    }

}
