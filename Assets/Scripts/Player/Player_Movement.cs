using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravity = 25f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 3f;
    [SerializeField] AnimationCurve rollCurve, stepbackCurve;

    private CharacterController controller;
    private Animator anim;
    private Transform cam;

    private float speedSmoothVelocity;
    private float speedSmoothTime;
    private float currentSpeed;
    private float velocityY;
    private Vector3 moveInput;
    private Vector3 direction;

    private bool isRolling, isAttacking, isBlocking;
    private float dodgeTimer, stepbackTimer, attackTimer;

    public bool lockRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;

        Keyframe rollLastFrame = rollCurve[rollCurve.length - 1];
        dodgeTimer = rollLastFrame.time;

        Keyframe stepbackLastFrame = stepbackCurve[rollCurve.length - 1];
        stepbackTimer = stepbackLastFrame.time;

        attackTimer = 1.07f;
    }

    private void Update()
    {
        GetMovementInput();

        Move();
        Rotate();

        CheckDodge();
        CheckAttack();
        CheckBlock();
    }

    private void GetMovementInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        direction = (forward * moveInput.y + right * moveInput.x).normalized;
    }

    private void Move()
    {
        if (!isRolling && !isAttacking && !isBlocking)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);

            if (velocityY > -10)
            {
                velocityY -= Time.deltaTime * gravity;
            }

            Vector3 velocity = (direction * currentSpeed) + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);

            anim.SetFloat("movement", direction.magnitude, 0.1f, Time.deltaTime);
            anim.SetFloat("horizontal", moveInput.x, 0.0f, Time.deltaTime);
            anim.SetFloat("vertical", moveInput.y, 0.0f, Time.deltaTime);
        }
    }

    private void Rotate()
    {
        if (direction.magnitude != 0 && !lockRotation && !isBlocking)
        {
            float rotationSpeed = rotateSpeed;
            if (isRolling) rotationSpeed = rotationSpeed/2;
            Vector3 rotDir = new Vector3(direction.x, direction.y, direction.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            if (lockRotation)
            {
                LockedDodge();
            }
            else if (direction.magnitude != 0)
            {
                StartCoroutine(Roll());
            }
            else
            {
                StartCoroutine(StepBack());
            }
        }
    }

    private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isRolling && !isAttacking)
        {
            StartCoroutine(WeakAttack());
        }
    }

    private void CheckBlock()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isRolling)
        {
            isBlocking = true;
        }
        else
        {
            Invoke("StopBlockingAnimation", 0.25f);
        }

        anim.SetBool("block", isBlocking);
    }

    private void StopBlockingAnimation()
    {
        isBlocking = false;
    }

    private void LockedDodge()
    {
        if (moveInput.x > 0)
        {
            StartCoroutine(StepRight());
        }
        else if (moveInput.x < 0)
        {
            StartCoroutine(StepLeft());
        }
        else if (moveInput.y > 0)
        {
            StartCoroutine(StepForward());
        }
        else
        {
            StartCoroutine(StepBack());
        }
    }

    private IEnumerator Roll()
    {
        anim.SetTrigger("roll");
        isRolling = true;
        float _timer = 0;

        while (_timer < dodgeTimer)
        {
            float speed = rollCurve.Evaluate(_timer);
            Vector3 dir = (transform.forward * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator StepBack()
    {
        anim.SetTrigger("stepback");
        isRolling = true;
        float _timer = 0;

        while (_timer < stepbackTimer)
        {
            float speed = stepbackCurve.Evaluate(_timer);
            Vector3 dir = (-transform.forward * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator StepForward()
    {
        anim.SetTrigger("stepback");
        isRolling = true;
        float _timer = 0;

        while (_timer < stepbackTimer)
        {
            float speed = stepbackCurve.Evaluate(_timer);
            Vector3 dir = (transform.forward * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator StepRight()
    {
        anim.SetTrigger("stepback");
        isRolling = true;
        float _timer = 0;

        while (_timer < stepbackTimer)
        {
            float speed = stepbackCurve.Evaluate(_timer) * 1.2f;
            Vector3 dir = (transform.right * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator StepLeft()
    {
        anim.SetTrigger("stepback");
        isRolling = true;
        float _timer = 0;

        while (_timer < stepbackTimer)
        {
            float speed = stepbackCurve.Evaluate(_timer) * 1.2f;
            Vector3 dir = (-transform.right * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator WeakAttack()
    {
        anim.SetTrigger("attack");
        isAttacking = true;
        float _timer = 0;

        while (_timer < attackTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }
}
