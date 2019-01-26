using UnityEngine;

public class Spike : MonoBehaviour
{
    public float speed = 5;

    public void Go(Vector2 initialPosition, Vector2 direction)
    {
        transform.position = initialPosition;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        GetComponentInChildren<SpriteRenderer>().transform.right = direction;
    }
}
