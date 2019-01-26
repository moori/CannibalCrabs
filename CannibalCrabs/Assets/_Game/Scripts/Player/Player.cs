using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int size {
        get { return 2; }
    }
    public int meatsCollected;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Eat()
    {
        var initSize = size;
        meatsCollected++;
        if(size> initSize)
        {
            //levelup
        }

    }

    internal void SetPlayer(int i)
    {
        GetComponent<PlayerInput>().playerString = $"P{i + 1}_";
    }

    public void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v).normalized * speed;
        if (h != 0)
        {
            sprite.flipX = h < 0;
        }
    }

}
