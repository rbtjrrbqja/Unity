using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveDot : MonoBehaviour
{
    [Range(0, 1)]
    public float time = 0;

    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;

    public GameObject Ba;
    public GameObject Bb;
    public GameObject Bc;

    public GameObject OnLineA;
    public GameObject OnLineB;

    public GameObject CurvePoint;

    private bool check = true;
    float speed = 0.1f;

    private void Update()
    {
        //optimizedBezierCurve(time);
        bezierCurve(time);
        if (check)
            time += speed * Time.deltaTime;
        else
            time -= speed * Time.deltaTime;

        if (time > 0.999f)
            check = false;

        if (time < 0)
            check = true;
    }

    Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return (1f - t) * a + t * b;
    }

    // 베지어 곡선
    private void bezierCurve(float t)
    {
        Ba.transform.position = Lerp(p0.position, p1.position, t);
        Bb.transform.position = Lerp(p1.position, p2.position, t);
        Bc.transform.position = Lerp(p2.position, p3.position, t);

        OnLineA.transform.position = Lerp(Ba.transform.position, Bb.transform.position, t);
        OnLineB.transform.position = Lerp(Bb.transform.position, Bc.transform.position, t);

        CurvePoint.transform.position = Lerp(OnLineA.transform.position, OnLineB.transform.position, t);
    }

    // 최적화된 베지어 곡선
    private void optimizedBezierCurve(float t)
    {
        float t1 = (1 - t); //  1 - t
        float t2 = t1 * t1; // (1 - t) ^ 2
        float t3 = t1 * t2; // (1 - t) ^ 3

        Vector2 movePoint = p0.transform.position * t3 + p1.transform.position * 3 * t2 * t +
            p2.transform.position * 3 * t1 * t * t + p3.transform.position * t * t * t;

        CurvePoint.transform.position = movePoint;
    }
}
