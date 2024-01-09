using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    [Header("Animation Curves")]
    [SerializeField] private AnimationCurve rollCurve;
    [SerializeField] private AnimationCurve stepbackCurve;
    [SerializeField] private AnimationCurve lightAttackCurve;
    [SerializeField] private AnimationCurve strongAttackCurve;

    [Header("Settings")]
    [SerializeField] private float weakAttackRadius;
    [SerializeField] private float health = 100f;

    [Header("Indicators")]
    [SerializeField] GameObject attackingIndicator;

    private Player_Movement playerMov;
    private Player_Behavior playerBeh;
    private Animator anim;

    private float velocityY;
    private float dodgeTimer, stepbackTimer, lightAttackTimer, strongAttackTimer;

    private float LIGHT_ATTACK_DURATION = 1.05f;

    private bool isRolling, isAttacking, isBlocking;

    public enum LightAttackState
    {
        started = 0,
        firstHit,
        secondHit,
        thirdHit,
        ended
    }

    private LightAttackState lightAttackState;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerMov = GetComponent<Player_Movement>();
        playerBeh = GetComponent<Player_Behavior>();

        Keyframe rollLastFrame = rollCurve[rollCurve.length - 1];
        dodgeTimer = rollLastFrame.time;

        //Keyframe stepbackLastFrame = stepbackCurve[stepbackCurve.length - 1];
        //stepbackTimer = stepbackLastFrame.time;

        Keyframe lightAttackLastFrame = lightAttackCurve[lightAttackCurve.length - 1];
        lightAttackTimer = lightAttackLastFrame.time;

        Keyframe strongAttackLastFrame = strongAttackCurve[strongAttackCurve.length - 1];
        strongAttackTimer = strongAttackLastFrame.time;
    }

    private void Update()
    {
        attackingIndicator.SetActive(isAttacking);
    }

    public void PlayLightAttack()
    {
        lightAttackTimer = lightAttackCurve.Evaluate(LIGHT_ATTACK_DURATION);
        StartCoroutine(LightAttack());
    }

    public void ContinueLightAttack(int comboState)
    {
        lightAttackTimer = lightAttackCurve.Evaluate(comboState * LIGHT_ATTACK_DURATION);
    }

    public void PlayStrongAttack()
    {
        StartCoroutine(StrongAttack());
    }

    public void PlayRoll()
    {
        StartCoroutine(Roll());
    }

    public void PlayMovement()
    {
        anim.SetFloat("velocity", playerMov.GetVelocity(), 0.1f, Time.deltaTime);
    }

    public void PlayBlock(bool play)
    {
        isBlocking = play;
        anim.SetBool("block", isBlocking);
    }

    private IEnumerator Roll()
    {
        anim.SetTrigger("roll");
        isRolling = true;
        float _timer = 0;

        while (_timer < dodgeTimer)
        {
            float speed = rollCurve.Evaluate(_timer);
            playerMov.PerformMoveForRolling(speed);
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
            //controller.Move(dir * Time.deltaTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        isRolling = false;
    }

    private IEnumerator LightAttack()
    {
        anim.SetBool("lightAttack", true);
        isAttacking = true;
        float _timer = 0;
        
        while (_timer < lightAttackTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        playerBeh.ResartLightCombo();
        anim.SetBool("lightAttack", false);
        isAttacking = false;
    }

    private IEnumerator StrongAttack()
    {
        anim.SetTrigger("strongAttack");
        isAttacking = true;
        float _timer = 0;

        while (_timer < strongAttackTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        
        isAttacking = false;
    }








    public void TakeDamage(float damageGiven)
    {
        health -= damageGiven;
        anim.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You are dead!");
    }




    public bool IsRolling()
    {
        return isRolling;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
