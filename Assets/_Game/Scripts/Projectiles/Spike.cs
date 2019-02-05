﻿using UnityEngine;

public class Spike : Projectile
{
    public float lifetime = 1;
    public override void Go(Player owner, Vector2 direction)
    {
        transform.position = (Vector2)owner.transform.position + direction * 0.5f;
        rb.velocity = direction * speed;
        sprite.transform.right = direction;
        sprite.color = owner.color;
        damage += owner.size * 0.25f;
        this.owner = owner;
        Destroy(gameObject, lifetime);
    }

    public override void Hit(Collider2D collision)
    {
        base.Hit(collision);

        hitEventEmitter.start();
    }

}