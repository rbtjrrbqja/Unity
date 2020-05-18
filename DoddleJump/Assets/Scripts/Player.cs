using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    // Control var player move and gravity
    public float jumpHeight;
    public float timeToJumpApex;
    public float accelerationTime;
    public float moveSpeed;

    private float gravity;
    private float jumpVelocity;
    private Vector2 velocity;

    // ref var smoothdamp
    private float velocityXSmoothing;

    private Controller2D controller;

    private float inputX;

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    private void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // h = g * t ^ 2 / 2
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;         // Vt = a * t;
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // gravity
        if (controller.collisions.below)
            velocity.y = jumpVelocity;

        velocity.x = Mathf.SmoothDamp(velocity.x, inputX * moveSpeed, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void GameOver()
    {
        velocity = Vector2.zero;
        gameObject.SetActive(false);
        transform.position = new Vector3(0, -2, -5);
    }

    public void Rerty()
    {
        gameObject.SetActive(true);
    }
}
