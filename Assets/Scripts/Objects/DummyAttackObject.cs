using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttackObject : MonoBehaviour
{
    [SerializeField] private float force;

    private Vector3 initPoint;
    private Rigidbody2D rb;
    
    public void SetInitialPoint(Vector3 init)
    {
        initPoint = init;

        transform.rotation = Quaternion.Euler(0, 0, -90);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -1).normalized * force;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, initPoint) >= 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //collision.gameObject.GetComponent<Player_Behavior>().TakeDamage(1);
            Destroy(gameObject);
        }
        else { /* No nothing*/ }
    }
}
