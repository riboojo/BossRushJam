using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravity = 25f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dodgeSpeed = 4f;
    [SerializeField] private float rotateSpeed = 3f;
    [SerializeField] AnimationCurve dodgeCurve, stepbackCurve;

    private CharacterController controller;
    private Animator anim;
    private Transform cam;

    private float speedSmoothVelocity;
    private float speedSmoothTime;
    private float currentSpeed;
    private float velocityY;
    private Vector3 moveInput;
    private Vector3 direction;

    private bool isDodging;
    private float dodgeTimer, stepbackTimer;

    public bool lockRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;

        Keyframe dodgeLastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodgeLastFrame.time;

        Keyframe stepbackLastFrame = stepbackCurve[dodgeCurve.length - 1];
        stepbackTimer = stepbackLastFrame.time;
    }

    private void Update()
    {
        GetInput();
        Move();
        Rotate();
        Dodge();
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

        direction = (forward * moveInput.y + right * moveInput.x).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && !lockRotation)
        {
            if (direction.magnitude != 0)
            {
                StartCoroutine(Dodge());
            }
            else
            {
                StartCoroutine(StepBack());
            }
        }
    }

    private void Move()
    {
        if (!isDodging)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);

            if (velocityY > -10)
            {
                velocityY -= Time.deltaTime * gravity;
            }

            Vector3 velocity = (direction * currentSpeed) + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            anim.SetFloat("movement", direction.magnitude, 0.1f, Time.deltaTime);
            anim.SetFloat("horizontal", moveInput.x, 0.1f, Time.deltaTime);
            anim.SetFloat("vertical", moveInput.y, 0.1f, Time.deltaTime);
        }
    }

    private void Rotate()
    {
        if (direction.magnitude != 0 && !lockRotation)
        {
            float rotationSpeed = rotateSpeed;
            if (isDodging) rotationSpeed = rotationSpeed/2;
            Vector3 rotDir = new Vector3(direction.x, direction.y, direction.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Dodge()
    {
        anim.SetTrigger("dodge");
        isDodging = true;
        float _timer = 0;

        while (_timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(_timer);
            Vector3 dir = (transform.forward * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }

    private IEnumerator StepBack()
    {
        anim.SetTrigger("stepback");
        isDodging = true;
        float _timer = 0;

        while (_timer < stepbackTimer)
        {
            float speed = stepbackCurve.Evaluate(_timer);
            Vector3 dir = (-transform.forward * speed) + Vector3.up * velocityY;
            controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }
}
