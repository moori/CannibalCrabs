using UnityEngine;

public abstract class Shell : MonoBehaviour
{
    public float[] maxHealth;
    public float hp;
    public int size;
    public float cooldownDuration;
    public System.Action<Shell, Player> OnEnterShell;
    public bool isFuderosao { get { return size >= maxSize; } }
    public static readonly int maxSize = 4;

    protected float timeLastShot;
    public bool canShoot => (Time.time - timeLastShot) >= cooldownDuration;
    protected Rigidbody2D rb;
    public Player owner;
    [HideInInspector]
    public SpriteRenderer sprite;
    protected Healthbar healthbar;

    private bool isEquipped;
    [FMODUnity.EventRef]
    public string shoot_sfx_event;


    public FMOD.Studio.EventInstance shootEventEmitter;
    public FMOD.Studio.EventInstance crackShellEventEmitter;

    public ParticleSystem crackShellPart;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        healthbar = GetComponentInChildren<Healthbar>();
        shootEventEmitter = FMODUnity.RuntimeManager.CreateInstance(shoot_sfx_event);
        crackShellEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/shell_crack");
    }

    public abstract void Shoot(Vector2 direction);


    public virtual void Sacrifice(Vector2 direction)
    {
        BreakShell();
    }

    public virtual void SetSize(int size)
    {
        transform.localScale = Vector3.one * Size.sizeScale[size];
        rb.mass = 3 + (size * 3);
        rb.drag = 1 + (size * 2);
    }

    public virtual void TakeDamage(float value)
    {
        if (isFuderosao)
            return;

        hp -= value;
        healthbar.UpdateFillBar(hp / maxHealth[size]);
        if (hp <= 0)
        {
            BreakShell();
        }
    }

    public virtual void BreakShell()
    {
        if (owner != null && owner.currentShell)
            owner.currentShell = null;

        crackShellPart.gameObject.SetActive(true);
        crackShellPart.gameObject.transform.SetParent(null);

        Destroy(gameObject);
    }

    public virtual void EnterShell(Player player)
    {
        if (isEquipped)
            return;

        isEquipped = true;
        rb.isKinematic = true;
        rb.GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        transform.SetParent(player.sprite.transform);
        transform.SetAsFirstSibling();
        transform.localPosition = new Vector3(0.93f, -0.5f, 0);
        player.EnterShell();
        player.currentShell = this;
        owner = player;
        healthbar.canvasGroup.alpha = 1;
        transform.localScale = new Vector3(Mathf.Sign(player.transform.localScale.x), 1, 1);
        OnEnterShell(this, player);
        if (size < 4)
            healthbar.UpdateFillBar(hp / maxHealth[size]);
    }

    public void Unequip(Player player)
    {
        player.currentShell = null;
        owner = null;
        transform.SetParent(null);
        isEquipped = false;
        rb.isKinematic = false;
        rb.GetComponent<Collider2D>().enabled = true;
    }

    public virtual void Push(Vector2 direction, float force)
    {
        if (owner != null || isFuderosao)
            return;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player.currentShell == null)
            {
                if (player.size == size)
                    EnterShell(player);
            }
        }
    }
}
