using UnityEngine;

public class NinjaBomb : MonoBehaviour
{
    public float duration = 2f;

    private Player ninjaPlayer;

    public void Activate(Player ninjaPlayer, Vector2 position)
    {
        this.ninjaPlayer = ninjaPlayer;
        transform.position = position;
        this.DelayedAction(duration, () => Destroy(gameObject));
    }

    private void Infect(Player player)
    {
        if (!player.isPoisoned)
            player.Infect(1f, 5);
        else if (player.poison.isHealing)
            player.poison.StopHealing();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player == ninjaPlayer)
            ninjaPlayer.SetVisibility(false);
        else
            Infect(player);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player == ninjaPlayer)
            ninjaPlayer.SetVisibility(true);
        else if (player.isPoisoned)
            player.poison.BeginHealing();
    }
}
