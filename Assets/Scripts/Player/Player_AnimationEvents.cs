using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationEvents : MonoBehaviour
{
    private Player_Behavior playerBeh;

    private void Start()
    {
        playerBeh = GetComponentInParent<Player_Behavior>();
    }

    public void LightAttackHit()
    {
        playerBeh.LigthAttackHit();
    }

    public void StrongAttackHit()
    {
        playerBeh.StrongAttackHit();
    }
}
