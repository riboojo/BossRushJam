using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Exchange : MonoBehaviour
{
    private Player_Health playerHealth;
    private Player_Meele playerMeele;

    private float specialStartTime = 0f;
    private float specialHoldTime = 1f;

    private float shieldStartTime = 0f;
    private float shieldHoldTime = 1f;

    private void Start()
    {
        playerHealth = GetComponent<Player_Health>();
        playerMeele = GetComponent<Player_Meele>();

        playerMeele.DeactivateShield();
        playerMeele.DeactivateSpecial();

    }

    private void Update()
    {
        CheckSpecial();
        CheckShield();
    }

    private void CheckSpecial()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            specialStartTime = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (specialStartTime + specialHoldTime <= Time.time)
            {
                ExchangeSpecial();
            }
        }
    }

    private void CheckShield()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shieldStartTime = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (shieldStartTime + shieldHoldTime <= Time.time)
            {
                ExchangeShield();
            }
        }
    }

    private void ExchangeSpecial()
    {
        if (!playerMeele.IsSpecialActive())
        {
            playerHealth.TakeDamage(2);
            playerMeele.ActivateSpecial();
        }
    }

    private void ExchangeShield()
    {
        if (!playerMeele.IsShieldActive())
        {
            playerHealth.TakeDamage(1);
            playerMeele.ActivateShield();
        }
    }
}
