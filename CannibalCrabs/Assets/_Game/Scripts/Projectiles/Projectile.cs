using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;

    protected Player owner;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Go(Player owner, Vector2 direction)
    {
    }

    public virtual void Hit(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == owner)
                return;

            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }
}
