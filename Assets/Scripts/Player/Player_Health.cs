using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [SerializeField] GameObject[] healthBlocks;
    
    private int maxHealth;

    private int health = 10;

    private void Start()
    {
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0)
        {
            for (int i = maxHealth; i > health; i--)
            {
                healthBlocks[i - 1].SetActive(false);
            }
        }
        else
        {
            /* Game Over */
            health = maxHealth;
            for (int i = 0; i < maxHealth; i++)
            {
                healthBlocks[i].SetActive(true);
            }
        }
    }
}
