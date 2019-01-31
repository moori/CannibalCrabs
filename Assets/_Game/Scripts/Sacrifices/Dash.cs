using UnityEngine;

public class Dash : MonoBehaviour
{
    public float damage = 10;
    public float speed = 5;

    private Player dasher;

    public void Activate(Player dasher, Vector2 initialPosition, Vector2 direction)
    {
        dasher.gameObject.SetActive(false);
        this.dasher = dasher;
        transform.position = initialPosition;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        rigidBody.velocity = direction * speed;
        sprite.transform.up = direction;
        transform.localScale = dasher.transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player == dasher)
                return;

            player.TakeDamage(damage);
        }

        dasher.gameObject.SetActive(true);
        dasher.transform.position = transform.position;
        Destroy(gameObject);
    }
}
