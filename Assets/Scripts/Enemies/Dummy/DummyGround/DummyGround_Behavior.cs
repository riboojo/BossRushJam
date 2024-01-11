using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGround_Behavior : Enemy_Behavior
{
    [SerializeField] private Player_Movement player;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private float jumpDistance = 250f;
    private float jumpHeight = 1500f;

    private bool isFacingLeft = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && IsGrounded())
        {
            JumpAttack();
        }
        else if (Input.GetKeyDown(KeyCode.O) && IsGrounded())
        {
            MacheteAttack();
        }
        else if (IsGrounded())
        {
            Flip();
            Idle();
        } else { }
    }

    private void Flip()
    {
        Vector3 playerPosition = player.GetCurrentPosition();

        if (playerPosition.x > transform.position.x && isFacingLeft || playerPosition.x <= transform.position.x && !isFacingLeft)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Idle()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void JumpAttack()
    {
        Vector3 playerPosition = player.GetCurrentPosition();

        if (playerPosition.x > transform.position.x)
        {
            rb.AddForce(new Vector2(jumpDistance, jumpHeight), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(-jumpDistance, jumpHeight), ForceMode2D.Impulse);
        }
    }

    private void MacheteAttack()
    {
        anim.SetTrigger("SimpleMachete");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_Behavior player = collision.gameObject.GetComponent<Player_Behavior>();
            player.GetHurt(transform.position - collision.transform.position);
        }
    }

    public override void Hurt()
    {

    }
}
