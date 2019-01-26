using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int size
    {
        get { return 2; }
    }
    public int meatsCollected;
    public Meat meatPrefab;
    public float afterHitInvulnerabilityDuration = 2;

    [HideInInspector]
    public Vector2 aimDirection = Vector2.right;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [HideInInspector]
    public Shell currentShell;

    public System.Action<Player> OnDie = (p) => { };

    private bool canTakeDamage = true;

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
        currentShell?.Shoot(aimDirection);
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
        for (int i = 0; i < meatsCollected + 1; i++)
        {
            var meat = Instantiate(meatPrefab);
            meat.transform.position = transform.position + ((Vector3)Random.insideUnitCircle * 2.5f);
        }
        OnDie(this);
        gameObject.SetActive(false);
    }

    public void Invulnerability(float duration)
    {
        canTakeDamage = false;
        this.DelayedAction(duration, () => canTakeDamage = true);
    }
}
