using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Behavior : Enemy_Behavior
{
    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public override void DamageTaken(int damage)
    {
        anim.Play();
    }

    public override void DamageTaken(int damage, int type)
    {
        throw new System.NotImplementedException();
    }
}
