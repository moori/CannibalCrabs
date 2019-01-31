using UnityEngine;

public class RolaThrow : MonoBehaviour
{
    public float damage = 0;
    public float speed = 5;

    private Player thrower;
    private Shell inactiveRola;

    public void Activate(Player thrower, Vector2 initialPosition, Vector2 direction)
    {
        inactiveRola = thrower.currentShell;
        thrower.currentShell = null;
        inactiveRola.gameObject.SetActive(false);
        this.thrower = thrower;
        transform.position = initialPosition;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        rigidBody.velocity = direction * speed;
        sprite.transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player == thrower || !player.hasShell)
                return;

            inactiveRola.gameObject.SetActive(true);
            Shell victimShell = player.currentShell;

            player.currentShell.Unequip(player);
            thrower.currentShell.Unequip(thrower);

            inactiveRola.EnterShell(player);
            victimShell.EnterShell(thrower);

            Debug.Log("kd", player);
        }
        else
        {
            Debug.Log("oxi", collision);
            inactiveRola.BreakShell();
        }
        
        Destroy(gameObject);
    }
}
