using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingLightning : MonoBehaviour
{

    public LineRenderer m_Line;
    [Range(0, 1)]
    public float NoiseSpeed = 0.05f;
    public Vector3 Center;
    public float Radius;
    [Range(0, 3)]
    public float MinAngles;
    public float MinOffset;
    // Use this for initializatin
    void Start()
    {
    }


    float loctime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (loctime >= NoiseSpeed)
        {
            InitLine(m_Line, MinAngles, MinOffset);
            loctime = 0;
        }
        else
        {
            loctime += Time.deltaTime;
        }
    }

    void InitLine(LineRenderer line, float minAngles, float minOffset)
    {

        List<Vector3> list = new List<Vector3>();

        minAngles = UnityEngine.Random.Range(minAngles * 1f, minAngles * 3.5f); //实现bilibili效果，偶有一次幅度要大一点的
        int count = (int)(360f / minAngles);

        Vector3 star = transform.up * Radius;
        list.Add(star);

        Vector3 dirct;
        Quaternion qua;

        for (int i = 1; i < count; i++)
        {
            qua = Quaternion.Euler(new Vector3(0f, 0f, minAngles * i));
            dirct = qua * star;
            dirct += RandomOffect(minOffset * 2 / 3);
            list.Add(dirct);
        }

        list.Add(star);
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
