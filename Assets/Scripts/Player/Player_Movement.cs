using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravity = 25f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 3f;

    private CharacterController controller;
    private Animator anim;
    private Transform cam;

    private float speedSmoothVelocity;
    private float speedSmoothTime;
    private float currentSpeed;
    private float velocityY;
    private Vector3 moveInput;
    private Vector3 dir;

    public bool lockMovement;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        GetInput();
        Move();
        Rotate();
    }

    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        dir = (forward * moveInput.y + right * moveInput.x).normalized;
    }

    private void Move()
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);

        if (velocityY > -10)
        {
            velocityY -= Time.deltaTime * gravity;
        }

        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        //anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
    }

    private void Rotate()
    {
        if (dir.magnitude != 0 && !lockMovement)
        {
            Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), rotateSpeed * Time.deltaTime);
        }
    }
}
