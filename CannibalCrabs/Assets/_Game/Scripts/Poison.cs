using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison
{
    private Player victim;
    private int poisonTicksCount;
    private float damage = 1;
    private float damageInterval = .2f;
    private int poisonTotalTicks = 10;
    public bool isHealing { get; private set; } = false;

    public Poison(Player victim, float duration, float totalDamage, int ticks = 10)
    {
        this.victim = victim;
        poisonTicksCount = 0;
        poisonTotalTicks = ticks;
        damageInterval = duration / ticks;
        damage = totalDamage / ticks;
        victim.DelayedAction(damageInterval, () => EmitDamage());
    }

    public void BeginHealing()
    {
        isHealing = true;
        poisonTicksCount = 0;
    }

    public void StopHealing()
    {
        isHealing = false;
        poisonTicksCount = 0;
    }

    public void Cure()
    {
        victim.poison = null;
    }

    private void EmitDamage()
    {
        if (victim.poison == null)
            return;

        if (isHealing)
            poisonTicksCount++;

        if (victim.gameObject.activeInHierarchy)
                victim.TakeDamage(damage);

        if (poisonTicksCount >= poisonTotalTicks || victim.currentShell == null)
            Cure();
        else
            victim.DelayedAction(damageInterval, () => EmitDamage());
    }
}
