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

        Debug.Log("chomp");
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(h, v).normalized * speed;
        //if (h != 0 && v != 0) {
        //    //rb.MovePosition(new Vector2(h,v).normalized * speed);
        //}
    }


}
