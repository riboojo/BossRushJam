using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy_Behavior : MonoBehaviour
{
    [SerializeField] public Image healthBar;

    public int health = 100;

    public abstract void Hurt();

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = health / 100f;
        Debug.Log(health);
    }

    public void TakeHeal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        healthBar.fillAmount = health / 100f;
    }
}
