using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowObejct : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector2 clampAxis = new Vector2(60f, 60f);

    [SerializeField] private float followSmoothing = 5f;
    [SerializeField] private float rotateSmoothing = 5f;
    [SerializeField] private float sensitivity = 60f;

    private float rotX, rotY;
    private bool cursorLocked = false;
    private Transform cam;

    public bool lockedTarget;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 targetP = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetP, followSmoothing * Time.deltaTime);

        if (!lockedTarget)
        {
            CameraTargetPosition();
        }
        else
        {
            LookAtTarget();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cursorLocked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void CameraTargetPosition()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotX += (mouseAxis.x * sensitivity) * Time.deltaTime;
        rotY -= (mouseAxis.y * sensitivity) * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, clampAxis.x, clampAxis.y);

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * rotateSmoothing);
    }

    private void LookAtTarget()
    {
        transform.rotation = cam.rotation;
        Vector3 r = cam.eulerAngles;
        rotX = r.y;
        rotY = 1.8f;
    }
}
