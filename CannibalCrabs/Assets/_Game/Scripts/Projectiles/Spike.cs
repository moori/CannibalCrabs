﻿using UnityEngine;

public class Spike : Projectile
{
    public float lifetime = 1;
    public override void Go(Player owner, Vector2 direction)
    {
        transform.position = (Vector2)owner.transform.position + direction * 0.5f;
        rb.velocity = direction * speed;
        sprite.transform.right = direction;
        this.owner = owner;
        Destroy(gameObject, lifetime);
    }

}
