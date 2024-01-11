using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorcycle : MonoBehaviour
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
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_Behavior>().TakeDamage(20);
            Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
