using UnityEngine;

public class Spike : Projectile
{
    private Player owner;

    public void Go(Player owner, Vector2 direction)
    {
        transform.position = (Vector2)owner.transform.position + direction * 0.5f;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        GetComponentInChildren<SpriteRenderer>().transform.right = direction;
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
}
