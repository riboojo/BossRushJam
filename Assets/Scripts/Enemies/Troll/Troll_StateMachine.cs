using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troll_StateMachine : MonoBehaviour
{
    [SerializeField] private float health = 10f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float followRange = 8f;

    [SerializeField] AnimationCurve weakAttackCurve;

    private float attackTimer;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator anim;

    private float newDestinationCooldown = 0.5f;
    private float timePassed = 0f;

    private bool isAttacking, damageGiven;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        Keyframe weakAttackLastFrame = weakAttackCurve[weakAttackCurve.length - 1];
        attackTimer = weakAttackLastFrame.time;
    }
    
    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        
        if (timePassed >= attackCooldown)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                timePassed = 0;
                StartCoroutine(WeakAttack());
            }

            if (newDestinationCooldown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= followRange && !isAttacking)
            {
                newDestinationCooldown = 0.5f;
                agent.SetDestination(player.transform.position);
            }
        }

        timePassed += Time.deltaTime;
        newDestinationCooldown -= Time.deltaTime;

        if ((agent.velocity.magnitude / agent.speed) > 0.5 && !isAttacking)
        {
            transform.LookAt(player.transform);
        }
    }

    private IEnumerator WeakAttack()
    {
        anim.SetTrigger("weakAttack");
        isAttacking = true;
        float _timer = 0;

        while (_timer < attackTimer)
        {
            bool hit = weakAttackCurve.Evaluate(_timer) >= 1;

            if (hit && !damageGiven)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                {
                    Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < 120.0f)
                    {
                        damageGiven = true;
                        player.GetComponent<Player_Movement>().TakeDamage(10f);
                    }
                }
            }

            _timer += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
        damageGiven = false;

        transform.LookAt(player.transform);
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
        Destroy(this.gameObject);
    }
}
