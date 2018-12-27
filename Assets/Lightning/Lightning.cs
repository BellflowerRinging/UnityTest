using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{

    // Use this for initialization
    public Transform m_Star;
    public Transform m_End;
    public LineRenderer m_Line;
    public int NodeCount;
    public float MaxOffset;
	[Range(0,1)]
    public float NoieSpeed = 0.2f;


    Vector3[] NodePostions;
    void Start()
    {
        NodePostions = InitLine(m_Line, m_Star.position, m_End.position, NodeCount);
        RandomPostions(m_Line, NodePostions, MaxOffset);
    }

    float loctime = 0f;
    // Update is called once per frame
    void Update()
    {
        if (loctime >= NoieSpeed)
        {
            RandomPostions(m_Line, NodePostions, MaxOffset);
            loctime = 0;
        }
        else
        {
            loctime += Time.deltaTime;
        }
    }

    Vector3[] InitLine(LineRenderer line, Vector3 star, Vector3 end, int nodeCount)
    {
        Vector3[] postions = new Vector3[nodeCount];
        line.positionCount = nodeCount;
        for (int i = 0; i < postions.Length; i++)
        {
            postions[i] = Vector3.Lerp(star, end, (float)i / (float)(nodeCount - 1));
        }
        line.SetPositions(postions);
        return postions;
    }

    void RandomPostions(LineRenderer line, Vector3[] nodePostions, float maxOffset)
    {
        Vector3[] postions = new Vector3[nodePostions.Length];
        Array.Copy(nodePostions, postions, nodePostions.Length);
        for (int i = 1; i < line.positionCount - 1; i++)
        {
            postions[i] += RandomOffect(maxOffset);
        }
        line.SetPositions(postions);
    }

    Vector3 RandomOffect(float maxOffset)
    {
        return new Vector3(UnityEngine.Random.Range(-maxOffset, maxOffset), UnityEngine.Random.Range(-maxOffset, maxOffset), UnityEngine.Random.Range(-maxOffset, maxOffset));
    }

}
