using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behavior : MonoBehaviour
{
    private float hitForceX = 5f;
    private float hitForceY = 5f;

    private Rigidbody2D rb;
    private Player_Movement playerMovement;
    private Player_Health playerHealth;
    private Player_Meele playerMeele;

    private bool canGetHurt = true;

    private float recoverTimer = 0.5f;
    private float recoverShieldTimer = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player_Movement>();
        playerHealth = GetComponent<Player_Health>();
        playerMeele = GetComponent<Player_Meele>();
    }

    public void GetHurt(Vector2 direction, int damage)
    {
        if (playerMeele.IsShieldActive())
        {
            playerMeele.DeactivateShield();
            StartCoroutine("RecoverFromShieldHurt");
        }
        else if (canGetHurt)
        {
            StartCoroutine("RecoverFromHurt");
            playerHealth.TakeDamage(damage);

            if (direction.x > 0)
            {
                rb.AddForce(new Vector2(-hitForceX, hitForceY), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
            }
        } else { }
    }

    private IEnumerator RecoverFromHurt()
    {
        float _timer = 0;

        canGetHurt = false;
        playerMovement.SetCanMove(false);

        while (_timer < recoverTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        playerMovement.SetCanMove(true);
        canGetHurt = true;
    }

    private IEnumerator RecoverFromShieldHurt()
    {
        float _timer = 0;

        canGetHurt = false;
        playerMovement.SetCanMove(false);

        while (_timer < recoverShieldTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        playerMovement.SetCanMove(true);
        canGetHurt = true;
    }

}
