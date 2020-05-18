using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector2[] localWayPoints;
    private Vector2[] globalWayPoints;

    public float speed;
    public float waitTime;
    public float easeAmount;

    private int fromWayPointsIndex;
    private float percentBetweenWayPoints;
    private float nextMoveTime;

    private Vector2 velocity;

    private void OnEnable()
    {
        globalWayPoints = new Vector2[localWayPoints.Length];

        for (int i = 0; i < localWayPoints.Length; i++)
            globalWayPoints[i] = localWayPoints[i] + (Vector2)transform.position;
    }

    private void Update()
    {
        velocity = CalculatePlatformMovement();
        transform.Translate(velocity);
    }

    private float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector2 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
            return Vector2.zero;

        fromWayPointsIndex %= globalWayPoints.Length;
        int toWayPointIndex = (fromWayPointsIndex + 1) % globalWayPoints.Length;

        float distanceBetweenWayPoints = Vector2.Distance(globalWayPoints[fromWayPointsIndex], globalWayPoints[toWayPointIndex]);

        percentBetweenWayPoints += Time.deltaTime * speed / distanceBetweenWayPoints;
        percentBetweenWayPoints = Mathf.Clamp01(percentBetweenWayPoints);

        float easedPercentBetweenWayPoints = Ease(percentBetweenWayPoints);

        Vector2 newPos = Vector2.Lerp(globalWayPoints[fromWayPointsIndex], globalWayPoints[toWayPointIndex], easedPercentBetweenWayPoints);
    
        if(percentBetweenWayPoints >= 1)
        {
            percentBetweenWayPoints = 0;
            fromWayPointsIndex++;
            
            if(fromWayPointsIndex >= globalWayPoints.Length - 1)
            {
                fromWayPointsIndex = 0;
                System.Array.Reverse(globalWayPoints);
            }
            
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - (Vector2)transform.position;
    }

}
