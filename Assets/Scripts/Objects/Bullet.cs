using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float force;
     
    private Vector3 mousePos;
    private Camera cam;
    private Rigidbody2D rb;

    private bool moving = false;
    private Vector3 endPoint;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        endPoint = direction;
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, endPoint) >= 30f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_Behavior>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (collision.tag == "Stuff")
        {
            Destroy(gameObject);
        }
        else { /* No nothing*/ }
    }
}
