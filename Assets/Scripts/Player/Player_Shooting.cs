using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefb;
    [SerializeField] private Transform bulletTransform;

    private float fireTimer;
    private float fireDebounce;

    private Vector3 mousePos;

    private bool isActive = true;

    private void Start()
    {

    }

    private void Update()
    {
        if (isActive)
        {
            HandleAiming();
            HandleShooting();
        }
    }

    private void HandleAiming()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefb, bulletTransform.position, Quaternion.identity);
        }
    }

    private void Shoot(Vector3 direction)
    {
        
    }




    public void SetActive(bool status)
    {
        isActive = status;
    }
}
