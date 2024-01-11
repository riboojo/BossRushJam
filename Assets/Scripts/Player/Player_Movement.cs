using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private Transform body;

    private float horizontal = 0f;
    private float speed = 8f;
    private float jumpPower = 16f;

    private bool isFacingRight = true;
    private bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        body = GameObject.FindGameObjectWithTag("PlayerBody").transform;
    }

    private void Update()
    {
        GetInputs();
        Flip();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        if (GetCanMove())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = body.transform.localScale;
            localScale.x *= -1f;
            body.localScale = localScale;
        }
    }



    public void SetCanMove(bool can)
    {
        canMove = can;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }
}
