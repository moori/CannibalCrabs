using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    public float damage;
    public float hp;
    public int size;
    public float cooldownDuration;
    public System.Action<Shell> OnEnterShell;

    protected float timeLastShot;
    protected bool canShoot => (Time.time - timeLastShot) >= cooldownDuration;
    protected Rigidbody2D rb;
    protected Player owner;
    protected SpriteRenderer sprite;

    private bool isEquipped;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public abstract void Shoot(Vector2 direction);


    public virtual void Sacrifice(Vector2 direction)
    {
        BreakShell();
    }

    public virtual void TakeDamage(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            BreakShell();
        }
    }

    public virtual void BreakShell()
    {
        owner.currentShell = null;
        Destroy(gameObject);
    }

    public virtual void EnterShell(Player player)
    {
        if (isEquipped)
            return;

        isEquipped = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.SetParent(player.sprite.transform);
        transform.localPosition = new Vector3(0.93f, -0.5f, 0);
        transform.localScale = Vector3.one;
        player.currentShell = this;
        sprite.color = player.color;
        owner = player;
        player.transform.localScale = Vector3.one * (1 + (size * 0.25f));
        OnEnterShell(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player.currentShell == null)
            {
                if (player.size == size)
                    EnterShell(player);
            }
            else if (owner == null)
            {
                if (isEquipped)
                    return;
                rb.AddForce((transform.position - player.transform.transform.position).normalized * 2, ForceMode2D.Impulse);
            }
        }
    }
}
