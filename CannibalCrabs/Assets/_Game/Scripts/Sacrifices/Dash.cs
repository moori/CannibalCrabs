using UnityEngine;

public class Dash : MonoBehaviour
{
    public float damage = 10;
    public float speed = 30;

    private Player dasher;

    public void Activate(Player dasher, Vector2 initialPosition, Vector2 direction)
    {
        dasher.Disable();
        this.dasher = dasher;
        transform.position = initialPosition;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        rigidBody.velocity = direction * speed;
        sprite.transform.right = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
        }

        dasher.Enable(transform.position);
    }
}
