using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 PlayerInput;
    public bool isMoving;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        isMoving = false;
        if (!isMoving)
        {
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if (PlayerInput != Vector2.zero)
            {
                animator.SetFloat("moveX", PlayerInput.x);
                animator.SetFloat("moveY", PlayerInput.y);

                var targetPos = transform.position;
                targetPos.x += PlayerInput.x;
                targetPos.y += PlayerInput.y;

                isMoving = true;
            }
        }

        animator.SetBool("isMoving", isMoving);
    }
    void FixedUpdate()
    {
        Vector2 moveForce = PlayerInput * moveSpeed;

        rb.velocity = moveForce;
    }

//
}