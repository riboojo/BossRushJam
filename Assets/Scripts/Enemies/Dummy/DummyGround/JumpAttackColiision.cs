using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackColiision : MonoBehaviour
{
    private DummyGround_Behavior dummyBehavior;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dummyBehavior = GetComponentInParent<DummyGround_Behavior>();

            if (dummyBehavior.isJumpAttacking)
            {
                dummyBehavior.JumpAttackHit();

                Player_Behavior player = collision.gameObject.GetComponent<Player_Behavior>();
                player.GetHurt(transform.position - collision.transform.position, 2);
            }
        }
    }
}
