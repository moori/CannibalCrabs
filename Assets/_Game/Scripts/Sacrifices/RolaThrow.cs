using FMODUnity;
using UnityEngine;

public class RolaThrow : MonoBehaviour
{
    public float damage = 0;
    public float speed = 5;

    private Player thrower;
    private Shell inactiveRola;
    [EventRef]
    public string sacrificeEvent;

    public void Activate(Player thrower, Vector2 initialPosition, Vector2 direction)
    {
        inactiveRola = thrower.currentShell;
        inactiveRola.gameObject.SetActive(false);
        this.thrower = thrower;
        transform.position = initialPosition;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        rigidBody.velocity = direction * speed;
        sprite.transform.up = direction;
        RuntimeManager.PlayOneShot(sacrificeEvent);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player == thrower || !player.hasShell)
                return;

            inactiveRola.gameObject.SetActive(true);
            Shell victimShell = player.currentShell;

            player.currentShell.Unequip(player);
            thrower.currentShell.Unequip(thrower);

            inactiveRola.EnterShell(player);
            if (thrower.currentShell == null)
                victimShell.EnterShell(thrower);
        }
        else
        {
            if (inactiveRola != null)
            {
                inactiveRola.transform.position = transform.position;
                inactiveRola.BreakShell();
            }
        }

        Destroy(gameObject);
    }
}
