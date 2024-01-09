using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll_Animations : MonoBehaviour
{
    [Header("Animation Curves")]
    [SerializeField] AnimationCurve tornadoAttackCurve;
    [SerializeField] AnimationCurve jumpAttackCurve;

    private float tornadoAttackTimer;
    private float jumpAttackTimer;

    private Animator anim;

    public enum JumpAttackState
    {
        jump = 0,
        land,
        attack,
        end
    }

    private JumpAttackState jumpAttack;

    void Start()
    {
        anim = GetComponent<Animator>();

        //Keyframe weakAttackLastFrame = tornadoAttackCurve[tornadoAttackCurve.length - 1];
        //tornadoAttackTimer = weakAttackLastFrame.time;

        Keyframe jumpAttackLastFrame = jumpAttackCurve[jumpAttackCurve.length - 1];
        jumpAttackTimer = jumpAttackLastFrame.time;
    }

    public void PlayGeneralHurt()
    {
        anim.SetTrigger("damage");
    }

    public void PlayWalk()
    {
        anim.SetBool("walk", true);
    }

    public void PlayIdle()
    {
        anim.SetBool("walk", false);
    }

    public void PlayJumpAttack()
    {
        StartCoroutine(JumpAttack());
    }

    public JumpAttackState GetJumpAttackState()
    {
        return jumpAttack;
    }

    private IEnumerator JumpAttack()
    {
        anim.SetTrigger("jumpAttack");
        jumpAttack = JumpAttackState.jump;
        float _timer = 0;

        while (_timer < jumpAttackTimer)
        {
            jumpAttack = (JumpAttackState)jumpAttackCurve.Evaluate(_timer);
            Debug.Log(jumpAttackCurve.Evaluate(_timer));
            _timer += Time.deltaTime;
            yield return null;
        }

        jumpAttack = JumpAttackState.end;
    }
}
