using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Meele : MonoBehaviour
{
    [SerializeField] private GameObject specialUI, shieldUI;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject motorcyclePrefab;

    private Player_Movement playerMovement;
    private Animator anim;

    private bool isShieldActive = false;
    private bool isSpecialActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player_Movement>();
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (IsSpecialActive())
            {
                Vector2 direction = new Vector3(1f, 0f);

                if (!playerMovement.IsFacingRight())
                {
                    direction.x *= -1;
                }

                GameObject motorcycle = Instantiate(motorcyclePrefab, transform.position, Quaternion.identity);
                motorcycle.GetComponent<Motorcycle>().SetDirection(direction);

                DeactivateSpecial();
            }
        }
    }




    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    public bool IsSpecialActive()
    {
        return isSpecialActive;
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        shieldUI.SetActive(true);
        shield.SetActive(true);
    }

    public void DeactivateShield()
    {
        isShieldActive = false;
        shieldUI.SetActive(false);
        shield.SetActive(false);
    }

    public void ActivateSpecial()
    {
        isSpecialActive = true;
        specialUI.SetActive(true);
    }

    public void DeactivateSpecial()
    {
        isSpecialActive = false;
        specialUI.SetActive(false);
    }
}
