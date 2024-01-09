using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Behavior : Enemy_Behavior
{
    [SerializeField] private GameObject dummyAttackPrefab;
    [SerializeField] private Transform attackPosition;
    
    private void Update()
    {
        if (Random.Range(0, 100) == 0)
        {
            DummyAttack();
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        
    }

    private void DummyAttack()
    {
        GameObject dummyAttack = Instantiate(dummyAttackPrefab, attackPosition.position, Quaternion.identity);
        dummyAttack.GetComponent<DummyAttackObject>().SetInitialPoint(attackPosition.position);
    }

    public override void Hurt()
    {
        
    }
}
