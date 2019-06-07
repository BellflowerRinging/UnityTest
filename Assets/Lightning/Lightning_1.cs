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
    public float SegmentLength;
    [Range(0, 1)]
    public float NoiseSpeed = 0.05f;


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
        if (loctime >= NoiseSpeed)
        {
            InitLine(m_Line, m_Star.position, m_End.position, SegmentLength);
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
        
        List<Vector3> list = new List<Vector3>();
        list.Add(star);

        msLength = UnityEngine.Random.Range(msLength * 0.5f, msLength * 2.5f); //实现bilibili效果，偶有一次幅度要大一点的
        
        Vector3 loc = star, dirct;
        while (Vector3.Distance(loc, end) >= msLength)
        {
            dirct = (end - loc).normalized * msLength;
            loc += dirct;
            loc += RandomOffect(msLength * 2 / 3); // 乘上2/3 效果要好一点
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
        return new Vector3(UnityEngine.Random.Range(-maxOffset, maxOffset),
         UnityEngine.Random.Range(-maxOffset, maxOffset), 
         UnityEngine.Random.Range(-maxOffset, maxOffset));
    }

}
