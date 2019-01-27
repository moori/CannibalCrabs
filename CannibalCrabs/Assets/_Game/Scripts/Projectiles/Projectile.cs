using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;

    protected Player owner;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;


    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance hitEventEmitter;
    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance missEventEmitter;
    public string hit_sfx_event;
    public string miss_sfx_event;

    public ParticleSystem hitPart;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        hitEventEmitter = FMODUnity.RuntimeManager.CreateInstance(hit_sfx_event);
        missEventEmitter = FMODUnity.RuntimeManager.CreateInstance(miss_sfx_event);
    }

    public virtual void Go(Player owner, Vector2 direction)
    {
    }

    public virtual void Hit(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == owner)
                return;

            player.TakeDamage(damage);
        }
        else
        {
            missEventEmitter.start();
        }
        //else if (collision.CompareTag("Shell"))
        //{
        //    Shell shell = collision.GetComponent<Shell>();
        //    shell.Push((collision.transform.position - transform.position).normalized, damage / 2);
        //    shell.TakeDamage(damage);
        //}
        hitPart.gameObject.SetActive(true);
        hitPart.gameObject.transform.SetParent(null);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }
}
