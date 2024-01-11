using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGroundProjectile : MonoBehaviour
{
    private float force = 20f;
    
    private Rigidbody2D rb;
    private Vector3 initPoint;

    public void SetDirection(Vector2 direction)
    {
        initPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * force;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, initPoint) >= 30f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player_Behavior player = collision.gameObject.GetComponent<Player_Behavior>();
            player.GetHurt(transform.position - collision.transform.position, 1);
            Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
