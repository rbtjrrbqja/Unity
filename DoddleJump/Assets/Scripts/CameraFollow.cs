using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Camera follows player, but don't move Sign(direction.y) == -1
    // just move up and side
    // Retry function reset camera position and reset focusArea

    // target is player
    public Controller2D target;

    // Camera's position.y from focusArea;
    public float verticalOffset;

    // Smooth time for Camera Moving
    public float verticalSmoothTime;
    public float horizontalSmoothTime;

    // player position check area, player == target
    public Vector2 focusAreaSize;

    // ref var for SmoothDamp function
    private float smoothVelocityX;
    private float smoothVelocityY;

    // save current focus Area and update.
    private FocusArea focusArea;

    private void Start()
    {
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        if (!target.gameObject.activeSelf)
            return;

        focusArea.Update(target.myCollider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        focusPosition.x = Mathf.SmoothDamp(transform.position.x, focusPosition.x, ref smoothVelocityX, horizontalSmoothTime);
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }
    
    // for debug
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;

        float left, right;
        float top, bottom;

        // initialize
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetbounds)
        {
            float shiftX = 0;

            if (targetbounds.min.x < left)
                shiftX = targetbounds.min.x - left;
            else if (targetbounds.max.x > right)
                shiftX = targetbounds.max.x - right;

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            if (targetbounds.max.y > top)
                shiftY = targetbounds.max.y - top;

            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    public void Retry()
    {
        transform.position = new Vector3(0, 2.5f, -10f);
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
    }
}
