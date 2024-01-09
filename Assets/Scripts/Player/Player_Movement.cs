using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravity = 25f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 3f;

    private Player_Animations playerAnim;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Transform cam;

    private float speedSmoothVelocity;
    private float speedSmoothTime;
    private float currentSpeed;
    private float velocityY;
    private Vector3 moveInput;
    private Vector3 direction;

    private bool isRolling, isAttacking, isBlocking;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Player_Animations>();

        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        playerInput.actions["Roll"].performed += ctx => CbkDodge();
    }

    private void Update()
    {
        GetMovementInput();
        GetAnimationState();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void GetMovementInput()
    {
        if (GetCanMove())
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            //moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); TODO: Use new Input system

            Vector3 forward = cam.forward;
            Vector3 right = cam.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            direction = (forward * moveInput.y + right * moveInput.x).normalized;
        }
        else
        {
            direction = Vector3.zero;
            playerAnim.PlayMovement();
        }
    }

    private void GetAnimationState()
    {
        isRolling = playerAnim.IsRolling();
        isBlocking = playerAnim.IsBlocking();
        isAttacking = playerAnim.IsAttacking();
    }

    private void Move()
    {
        if (GetCanMove())
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);

            if (velocityY > -10)
            {
                velocityY -= Time.deltaTime * gravity;
            }

            Vector3 velocity = (direction * currentSpeed) + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);

            playerAnim.PlayMovement();
        }
    }

    private void Rotate()
    {
        if (GetCanRotate())
        {
            Vector3 rotDir = new Vector3(direction.x, direction.y, direction.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), rotateSpeed * Time.deltaTime);
        }
    }

    private void CbkDodge()
    {
        if (direction.magnitude != 0 && !isRolling)
        {
            playerAnim.PlayRoll();
        }
        else
        {
            //Stepback
        }
    }

    public void PerformMoveForRolling(float speed)
    {
        Vector3 dir = (transform.forward * speed) + Vector3.up * velocityY;
        controller.Move(dir * Time.deltaTime);
    }

    public bool GetCanMove()
    {
        if (!isRolling && !isBlocking && !isAttacking)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetCanRotate()
    {
        if (direction.magnitude != 0 && !isRolling && !isBlocking && !isAttacking)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetVelocity()
    {
        return direction.magnitude;
    }
}
