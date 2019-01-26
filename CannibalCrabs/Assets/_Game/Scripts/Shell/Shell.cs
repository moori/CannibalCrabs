using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    public float damage;
    public float hp;
    public int size;

    public virtual void Shoot(Vector2 direction)
    {

    }

    public virtual void Sacrifice(Vector2 direction)
    {

    }

    public virtual void TakeDamage(float value)
    {

    }

    public virtual void BreakShell()
    {

    }
}
