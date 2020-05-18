using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;
    public int verticalRayCount;
    public CollisionInfo collisions;

    [HideInInspector]
    public BoxCollider2D myCollider;

    private const float skinWidth = 0.015f;
    private float verticalRaySpcing;
    private RaycastOrigin raycastOrigin;

    public UnityEvent OnPlayerDead;

    // GameOver()
    // UIManager.GameOver()       -> Show Retry Button
    // Player.GameOver()          -> Player's Velocity set Vector2.zero and SetActive(false)

    // Retry()
    // UIManager.Retry()          -> Score set 0 and Retry Button false
    // ObjectPooling.Retry()      -> Reset All Platforms Position set Vector2.zero and SetActive(false)    
    // CameraFollow.Retry()       -> Camera position set new Vector3(0, 2.5f, -10f) and Reset focusArea
    // Player.Retry()             -> Player true
    // GameManager.Retry()        -> idx = 0 and GenreateRandomPlatform(0, 40);


    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CalculateRaySpacing();
    }

    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.y != 0)
            VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }    

    private void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigin.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpcing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit)
            {
                // Ignore collision whenever player jump through platform
                if(hit.collider.tag == "Through")
                    if (directionY == 1 || hit.distance == 0)
                        continue;

                // game over
                if (hit.collider.tag == "BottomLimitLine")
                {
                    OnPlayerDead.Invoke();
                    break;
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = true;
            }
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        verticalRaySpcing = bounds.size.x / (verticalRayCount - 1);
    }


    struct RaycastOrigin
    {
        public Vector2 bottomLeft;
    }

    public struct CollisionInfo
    {
        public bool below;

        public void Reset()
        {
            below = false;
        }
    }
}
