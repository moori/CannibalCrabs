using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    public float damage;
    public float hp;
    public int size;
    public float cooldownDuration;

    protected float timeLastShot;
    protected bool canShoot => (Time.time - timeLastShot) >= cooldownDuration;
    protected Rigidbody2D rb;
    protected Player owner;
    protected SpriteRenderer sprite;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public abstract void Shoot(Vector2 direction);


    public virtual void Sacrifice(Vector2 direction)
    {
        BreakShell();
    }

    public virtual void TakeDamage(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            BreakShell();
        }
    }

    public virtual void BreakShell()
    {
        owner.currentShell = null;
        Destroy(gameObject);
    }

    public virtual void EnterShell(Player player)
    {
        rb.isKinematic = true;
        transform.position = player.transform.position + (Vector3.up * 0.8f);
        transform.SetParent(player.transform);
        player.currentShell = this;
        owner = player;
        player.transform.localScale = Vector3.one * (1 + (size * 0.25f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player.currentShell == null)
            {
                if (player.size == size)
                    EnterShell(player);
            }
            else if (owner == null)
            {
                rb.AddForce((transform.position - player.transform.transform.position).normalized * 2, ForceMode2D.Impulse);
            }
        }
    }
}
