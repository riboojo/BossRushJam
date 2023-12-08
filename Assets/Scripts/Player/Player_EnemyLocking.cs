using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EnemyLocking : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Transform enemyTargetLocator;
    [SerializeField] private Animator cinemachineAnim;

    [Header("Settings")]
    [SerializeField] private bool zeroVertLook;
    [SerializeField] private float noticeZone = 10f;
    [SerializeField] private float lookAtSoothing = 2f;
    [SerializeField] private float maxNoticeAngle = 60f;
    [SerializeField] private float crossHairScale = 0.1f;

    [SerializeField] Camera_FollowObejct camFollow;
    [SerializeField] Transform lockOnCanvas;

    private Transform cam;
    private bool enemyLocked;
    private float currentYOffset;
    private Vector3 pos;
    private Player_Movement playerMovement;

    private Transform currentTarget;
    private Animator anim;

    private void Start()
    {
        playerMovement = GetComponent<Player_Movement>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        camFollow.lockedTarget = enemyLocked;
        playerMovement.lockMovement = enemyLocked;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentTarget)
            {
                ResetTarget();
                return;
            }

            if (currentTarget = ScanNearBy())
            {
                FoundTarget();
            }
            else
            {
                ResetTarget();
            }
        }

        if (enemyLocked)
        {
            if (!TargetOnRange())
            {
                ResetTarget();
            }

            LookAtTarget();
        }
    }

    private void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true);
        anim.SetLayerWeight(1, 1);
        cinemachineAnim.Play("TargetCamera");
        enemyLocked = true;
    }

    private void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        currentTarget = null;
        enemyLocked = false;
        anim.SetLayerWeight(1, 0);
        cinemachineAnim.Play("FollowCamera");
    }

    private Transform ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        if (nearbyTargets.Length > 0)
        {
            for (int i = 0; i < nearbyTargets.Length; i++)
            {
                Vector3 dir = nearbyTargets[i].transform.position - cam.position;
                dir.y = 0;
                float _angle = Vector3.Angle(cam.forward, dir);

                if (_angle < closestAngle)
                {
                    closestTarget = nearbyTargets[i].transform;
                    closestAngle = _angle;
                }
            }

            if (closestTarget != null)
            {
                float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
                float h2 = closestTarget.localScale.y;
                float h = h1 * h2;
                float half_h = (h / 2) / 2;
                currentYOffset = h - half_h;

                if (zeroVertLook && currentYOffset > 1.6f && currentYOffset < 1.6f * 3)
                {
                    currentYOffset = 1.6f;
                }

                Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

                if (!Blocked(tarPos))
                {
                    return closestTarget;
                }
            }
        }

        return null;
    }

    private bool Blocked(Vector3 tar)
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, tar, out hit))
        {
            if (!hit.transform.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    private bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude;

        if (dis/2 > noticeZone)
        {
            return false;
        }

        return true;
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
        }
        else
        {
            //pos = currentTarget.position + new Vector3(0f, currentYOffset, 0f);
            pos = new Vector3(currentTarget.position.x, currentYOffset, currentTarget.position.z);
            lockOnCanvas.position = pos;
            lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHairScale);

            enemyTargetLocator.position = pos;
            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSoothing);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
