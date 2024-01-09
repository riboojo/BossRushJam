using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Behavior : MonoBehaviour
{
    public abstract void DamageTaken(int damage);
    public abstract void DamageTaken(int damage, int type);
}
