using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    private float hitForceX = 5f;
    private float hitForceY = 5f;

    private Rigidbody2D rb;
    private Player_Movement playerMovement;

    private bool canGetHurt = true;

    private float recoverTimer = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player_Movement>();
    }

    public void GetHurt(Vector2 direction)
    {
        if (canGetHurt)
        {
            StartCoroutine("BlockMovement");

            if (direction.x > 0)
            {
                rb.AddForce(new Vector2(-hitForceX, hitForceY), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator BlockMovement()
    {
        float _timer = 0;

        playerMovement.SetCanMove(false);

        while (_timer < recoverTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        playerMovement.SetCanMove(true);
    }

}
