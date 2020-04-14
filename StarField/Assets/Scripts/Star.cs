using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float pz { get; set; }

    public LineRenderer myLineRenderer;

    public void Init(float width, float height)
    {
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.05f;
        myLineRenderer.endWidth = 0.05f;

        x = Random.Range(-width / 2, width / 2);
        y = Random.Range(-height / 2, height / 2);
        z = Random.Range(0, width);
        pz = z;
    }
}
