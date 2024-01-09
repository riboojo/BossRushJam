using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Behavior : MonoBehaviour
{
    [Header("Fixed")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Settings")]
    [SerializeField] private float lightAttackRadius;
    [SerializeField] private float strongAttackRadius;
    [SerializeField] private int lightAttackDamage;
    [SerializeField] private int strongAttackDamage;

    [Header("Debug Indicators")]
    [SerializeField] GameObject enemyHitIndicator;

    private Player_Animations playerAnim;
    private PlayerInput playerInput;
    private Vector3 direction;
    private Vector3 moveInput;

    private bool isAttacking, isBlocking, isComboBusy;

    private enum LightCombo
    {
        started = 0,
        hit1,
        hit2,
        hit3,
        ended
    }

    private LightCombo lightComboState;



    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Player_Animations>();

        playerInput.actions["LightAttack"].performed += ctx => CbkLightAttack();
        playerInput.actions["StrongAttack"].performed += ctx => CbkStrongAttack();

        playerInput.actions["Block"].performed += ctx => CkbBlockStart();
        playerInput.actions["Block"].canceled += ctx => CkbBlockEnd();

        lightComboState = LightCombo.ended;
    }
    


    private void CbkLightAttack()
    {
        if (!playerAnim.IsBlocking() && !playerAnim.IsRolling() && !isComboBusy)
        {
            switch (lightComboState)
            {
                case LightCombo.ended:
                    isComboBusy = true;
                    lightComboState = LightCombo.hit1;
                    playerAnim.PlayLightAttack();
                    break;
                case LightCombo.hit2:
                    isComboBusy = true;
                    playerAnim.ContinueLightAttack((int)lightComboState);
                    break;
                case LightCombo.hit3:
                    isComboBusy = true;
                    playerAnim.ContinueLightAttack((int)lightComboState);
                    break;
                default:
                    break;
            }
        }
    }

    private void CbkStrongAttack()
    {
        if (!playerAnim.IsBlocking() && !playerAnim.IsRolling() && !playerAnim.IsAttacking())
        {
            playerAnim.PlayStrongAttack();
        }
    }


    private void CkbBlockStart()
    {
        if (!playerAnim.IsRolling())
        {
            playerAnim.PlayBlock(true);
        }
    }

    private void CkbBlockEnd()
    {
        playerAnim.PlayBlock(false);
    }



    public void LigthAttackHit()
    {
        LookForEnemies(lightAttackRadius, lightAttackDamage);

        switch (lightComboState)
        {
            case LightCombo.hit1:
                isComboBusy = false;
                lightComboState = LightCombo.hit2;
                break;
            case LightCombo.hit2:
                isComboBusy = false;
                lightComboState = LightCombo.hit3;
                break;
            case LightCombo.hit3:
                isComboBusy = false;
                lightComboState = LightCombo.ended;
                break;
            default:
                break;
        }
    }

    public void StrongAttackHit()
    {
        LookForEnemies(strongAttackRadius, strongAttackDamage);
    }

    public void ResartLightCombo()
    {
        isComboBusy = false;
        lightComboState = LightCombo.ended;
    }



    public void DamageTaken(int damage)
    {

    }



    private void LookForEnemies(float radius, int damage)
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (nearbyEnemies.Length > 0)
        {
            for (int i = 0; i < nearbyEnemies.Length; i++)
            {
                if (nearbyEnemies[i].transform.TryGetComponent(out Enemy_Behavior enemy))
                {
                    Vector3 directionToTarget = (nearbyEnemies[i].transform.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < 45.0f)
                    {
                        enemy.DamageTaken(damage);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, lightAttackRadius);
    }

}
