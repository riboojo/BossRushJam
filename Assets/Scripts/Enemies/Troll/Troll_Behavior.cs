using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troll_Behavior : Enemy_Behavior
{
    [Header("Settings")]
    [SerializeField] private float health;

    [Header("General Combat")]
    [SerializeField] private float walkVelocity;
    [SerializeField] private float attackRadius;

    [Header("Jump Attack")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attackVelocity;

    private GameObject player;
    private Transform jumpAttackTarget;

    private Troll_Animations trollAnim;

    private Rigidbody rb;

    private float originalY;

    public enum State
    {
        Idle = 0,
        Walk,
        Tornado,
        Jump
    }

    public enum JumpAttack
    {
        Enter = 0,
        Playing,
        Exit,
        None
    }

    private State state = State.Idle;
    private JumpAttack jumpAttackState = JumpAttack.None;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        jumpAttackTarget = GameObject.FindWithTag("JumpAttackTarget").transform;
        trollAnim = GetComponent<Troll_Animations>();
        rb = GetComponent<Rigidbody>();

        originalY = transform.position.y;
    }
    
    void Update()
    {
        UpdateState();
        StateMachine();
    }



    private void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetState(State.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            SetState(State.Tornado);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SetState(State.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            SetState(State.Idle);
        }
        else { /* Do nothing */ }
    }

    private void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                StateIdle();
                break;
            case State.Walk:
                StateWalkToPlayer();
                break;
            case State.Tornado:
                StateTornadoAttack();
                break;
            case State.Jump:
                StateJumpAttack();
                break;
            default:
                break;
        }
    }



    private void StateTornadoAttack()
    {
        
    }

    private void StateJumpAttack()
    {
        switch (jumpAttackState)
        {
            case JumpAttack.Enter:
                OnEnterJumpAttack();
                break;
            case JumpAttack.Playing:
                OnPlayingJumpAttack();
                break;
            case JumpAttack.Exit:
                OnExitJumpAttack();
                break;
            case JumpAttack.None:
                break;
            default:
                break;
        }
    }

    private void StateWalkToPlayer()
    {
        trollAnim.PlayWalk();

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        if (attackRadius >= Vector3.Distance(transform.position, target))
        {
            int attack = Random.Range((int)State.Tornado, (int)State.Jump);
            trollAnim.PlayIdle();
            SetState((State)attack);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, walkVelocity * Time.deltaTime);
            transform.LookAt(target);
        }
    }

    private void StateIdle()
    {
        trollAnim.PlayIdle();
    }


    private void OnEnterJumpAttack()
    {
        jumpAttackState = JumpAttack.Playing;
        trollAnim.PlayJumpAttack();
    }

    private void OnPlayingJumpAttack()
    {
        Vector3 target = new Vector3(jumpAttackTarget.position.x, originalY, jumpAttackTarget.position.z);

        if (trollAnim.GetJumpAttackState() == Troll_Animations.JumpAttackState.jump)
        {
            Debug.Log("OnPlayingJumpAttack_Jump");
            if (transform.position.y <= jumpHeight)
            {
                rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            }
            
            transform.LookAt(target);
        }
        else if (trollAnim.GetJumpAttackState() == Troll_Animations.JumpAttackState.land)
        {
            Debug.Log("OnEnterJumpAttack_Land");
            DisableCollider();
            transform.position = Vector3.MoveTowards(transform.position, target, attackVelocity * Time.deltaTime);
            transform.LookAt(target);
        }
        else if (trollAnim.GetJumpAttackState() == Troll_Animations.JumpAttackState.attack)
        {
            EnableCollider();
            Debug.Log("OnEnterJumpAttack_Attack");
        }
        else
        {
            jumpAttackState = JumpAttack.Exit;
        }
    }

    private void OnExitJumpAttack()
    {
        // ReturnToDefaultPosition()

        jumpAttackState = JumpAttack.None;
    }


    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                break;
            case State.Walk:
                break;
            case State.Tornado:
                break;
            case State.Jump:
                jumpAttackState = JumpAttack.Enter;
                break;
            default:
                break;
        }

        state = newState;
    }



    public override void DamageTaken(int damage)
    {
        trollAnim.PlayGeneralHurt();

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public override void DamageTaken(int damage, int type)
    {
        throw new System.NotImplementedException();
    }



    private void EnableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void DisableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = false; ;
    }



    private void Die()
    {
        Destroy(this.gameObject);
    }
}
