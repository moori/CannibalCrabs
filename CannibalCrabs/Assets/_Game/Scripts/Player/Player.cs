﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Color[] colors;
    [HideInInspector]
    public Color color;

    public int size
    {
        get { return sizeProgression.IndexOf(sizeProgression.First(value => meatsCollected < value)); }
    }
    public int meatsCollected;
    public Meat meatPrefab;
    public float afterHitInvulnerabilityDuration = 2;

    [HideInInspector]
    public Vector2 aimDirection = Vector2.right;

    private Rigidbody2D rb;
    [HideInInspector]
    public SpriteRenderer sprite;
    public GameObject bubblesParticles;
    private List<int> sizeProgression = new List<int>() { 3, 7, 12, 20, 999 };

    [HideInInspector]
    public Shell currentShell;

    public System.Action<Player> OnDie = (p) => { };

    private bool canTakeDamage = true;
    public bool canEat => currentShell != null ? meatsCollected < sizeProgression[currentShell.size] : true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Eat()
    {
        var initSize = size;
        meatsCollected++;
        if (size > initSize)
        {
            //levelup
            Debug.Log("LEVEL UP -> " + size);
        }

    }

    public void SetPlayer(int i)
    {
        GetComponent<PlayerInput>().playerString = $"P{i + 1}_";
        color = colors[i];
        sprite.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(sprite => sprite.color = color);
    }

    public void EnterShell()
    {
        bubblesParticles.SetActive(true);
    }

    public void Shoot()
    {
        currentShell?.Shoot(aimDirection);
    }

    public void Sacrifice()
    {
        if (currentShell != null)
        {
            currentShell.Sacrifice(aimDirection);
        }
    }

    public void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v).normalized * speed * (currentShell != null ? 1 / (1.5f + currentShell.size) : 1);
        if (h != 0)
        {
            sprite.transform.localScale = new Vector3(Mathf.Abs(sprite.transform.localScale.x) * (h > 0 ? -1 : 1), sprite.transform.localScale.y, sprite.transform.localScale.z);
        }
    }

    public void Aim(float h, float v, float aim_h, float aim_v)
    {
        Vector2 dir = new Vector2(aim_h, aim_v).normalized;
        if (dir.sqrMagnitude == 0)
        {
            dir = new Vector2(h, v).normalized;
            if (dir.sqrMagnitude == 0)
            {
                dir = aimDirection;
            }
        }

        aimDirection = dir;
    }

    public void TakeDamage(float damage)
    {
        if (currentShell != null)
        {
            if (canTakeDamage)
            {
                currentShell.TakeDamage(damage);
                Invulnerability(afterHitInvulnerabilityDuration);
            }
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        for (int i = 0; i < meatsCollected + 2; i++)
        {
            var meat = Instantiate(meatPrefab);
            meat.transform.position = transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * 2.5f);
        }
        OnDie(this);
        gameObject.SetActive(false);
    }

    public void SetImmunity(bool isImmune)
    {
        canTakeDamage = !isImmune;
    }

    public void Invulnerability(float duration)
    {
        canTakeDamage = false;
        this.DelayedAction(duration, () => canTakeDamage = true);
    }

}
