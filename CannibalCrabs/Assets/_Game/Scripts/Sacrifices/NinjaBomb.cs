using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaBomb : MonoBehaviour
{
    public float damage = 10;

    private Player protectedPlayer;
    private List<Player> playersInside = new List<Player>();

    public void Activate(Player protectedPlayer, Vector2 position)
    {
        this.protectedPlayer = protectedPlayer;
        transform.position = position;
        this.DelayedAction(2, () => Explode());
    }

    private void Explode()
    {
        protectedPlayer.SetImmunity(false);
        foreach (Player player in playersInside)
            player.TakeDamage(damage);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player == protectedPlayer)
            protectedPlayer.SetImmunity(true);
        else if (!playersInside.Contains(player))
            playersInside.Add(player);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player == protectedPlayer)
            protectedPlayer.SetImmunity(false);
        else if (playersInside.Contains(player))
            playersInside.Remove(player);
    }
}
