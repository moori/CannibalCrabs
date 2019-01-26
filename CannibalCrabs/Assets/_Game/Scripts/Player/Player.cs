using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int size
    {
        get { return 2; }
    }
    public int meatsCollected;

    [HideInInspector]
    public Vector2 aimDirection = Vector2.right;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public Shell currentShell;

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
        }

    }

    public void SetPlayer(int i)
    {
        GetComponent<PlayerInput>().playerString = $"P{i + 1}_";
    }

    public void Shoot()
    {
        currentShell.Shoot(aimDirection);
    }

    public void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v).normalized * speed;
        if (h != 0)
        {
            sprite.flipX = h < 0;
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
            currentShell.TakeDamage(damage);
        }
        else
        {
            Die();
        }
    }


    public void Die()
    {
        Debug.Log("FALOWWWW BJus");
        gameObject.SetActive(false);
    }
}
