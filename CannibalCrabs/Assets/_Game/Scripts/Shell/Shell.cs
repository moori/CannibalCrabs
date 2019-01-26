using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    public float damage;
    public float hp;
    public int size;
    public float cooldownDuration;

    protected float timeLastShot;
    protected bool canShoot => (Time.time - timeLastShot) >= cooldownDuration;
    protected Rigidbody2D rb;
    protected Player owner;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Shoot(Vector2 direction);


    public virtual void Sacrifice(Vector2 direction)
    {

    }

    public virtual void TakeDamage(float value)
    {

    }

    public virtual void BreakShell()
    {

    }

    public virtual void EnterShell()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player.currentShell == null)
            {
                rb.isKinematic = true;
                transform.position = player.transform.position + (Vector3.up * 0.8f);
                transform.SetParent(player.transform);
                player.currentShell = this;
                owner = player;
            }
            else if (owner == null)
            {
                rb.AddForce((transform.position - player.transform.transform.position).normalized * 3, ForceMode2D.Impulse);
            }
        }
    }
}
