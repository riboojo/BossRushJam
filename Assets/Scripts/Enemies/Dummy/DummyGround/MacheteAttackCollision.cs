using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacheteAttackCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_Behavior player = collision.gameObject.GetComponent<Player_Behavior>();
            player.GetHurt(transform.position - collision.transform.position, 1);
        }
    }
}
