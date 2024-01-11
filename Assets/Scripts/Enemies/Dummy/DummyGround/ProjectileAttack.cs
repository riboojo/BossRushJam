using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public void MacheteHitsGround()
    {
        GetComponent<DummyGround_Behavior>().CreateProjectile();
    }
}
