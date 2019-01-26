using UnityEngine;

public class Meat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player.canEat)
            {
                player.Eat();
                Destroy(gameObject);
            }
        }
    }
}
