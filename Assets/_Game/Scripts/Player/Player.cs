using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public UnityEngine.Color color;

    [HideInInspector]
    public Vector2 aimDirection = Vector2.right;

    public GameObject sprite;
    public List<SpriteRenderer> colorSprites;

    [HideInInspector]
    public Shell currentShell;

    public float speed;
    public int hp = 1;
    public List<Sprite> meatSprites;
    public int size => sizeProgression.IndexOf(sizeProgression.First(value => meatsCollected < value));
    public int meatsCollected;
    public Meat meatPrefab;
    public float afterHitInvulnerabilityDuration = 2;
    private Rigidbody2D rb;
    public GameObject bubblesParticles;
    private List<int> sizeProgression = new List<int>() { 3, 7, 12, 20, 999 };
    public Action<Player> OnDie = (p) => { };
    public Poison poison;
    public bool isPoisoned { get { return poison != null; } }
    public bool hasShell { get { return currentShell != null; } }
    private bool canTakeDamage = true;
    private bool isVisible = true;
    public bool canEat => currentShell != null ? meatsCollected < sizeProgression[currentShell.size] : true;
    private Animator animator;

    public ParticleSystem sandPart;
    public ParticleSystem disconfortPart;
    public ParticleSystem smokePart;

    public FMOD.Studio.EventInstance deathEventEmitter;
    public FMOD.Studio.EventInstance eatEventEmitter;
    public FMOD.Studio.EventInstance levelUpEventEmitter;
    public FMOD.Studio.EventInstance enterShellEventEmitter;
    public FMOD.Studio.EventInstance exitShellEventEmitter;
    public FMOD.Studio.EventInstance walkEventEmitter;
    private string[] walkEvents = new string[] { "event:/SndFx/crabs_slow_4", "event:/SndFx/crabs_slow_3", "event:/SndFx/crabs_slow_2", "event:/SndFx/crabs_slow_1" };

    public static List<Color> colors = new List<Color>() {
        new Color(239f/255f, 81f/255f, 81f/255f),
        new Color(255f/255f, 211f/255f, 25f/255f),
        new Color(129f/255f, 61f/255f, 180f/255f),
        new Color(253f/255f, 99f/255f, 230f/255f), // pink
        //new Color(81f/255f, 239f/255f, 227f/255f), // roxo
        new Color(61f/255f, 255f/255f, 0f/255f),
        new Color(58f/255f, 61f/255f, 115f/255f),
        new Color(58f/255f, 61f/255f, 115f/255f),
        new Color(255f/255f, 255f/255f, 255f/255f),
        new Color(168f/255f, 168f/255f, 168f/255f)
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        deathEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_death");
        eatEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_collect");
        levelUpEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_level_up");
        enterShellEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_shell_on");
        exitShellEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_shell_off");
        walkEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_slow_1");

        Rescale();
    }

    public void Eat()
    {
        int initSize = size;
        meatsCollected++;
        if (size > initSize)
        {
            //levelup
            Debug.Log("LEVEL UP -> " + size);
            if (size == 4)
            {
                levelUpEventEmitter = FMODUnity.RuntimeManager.CreateInstance("event:/SndFx/crabs_level_final");
            }
            //FMODUnity.RuntimeManager.PlayOneShot(, DamageEvent, transform.position);
            levelUpEventEmitter.start();
            levelUpEventEmitter.release();
            if (currentShell != null)
                disconfortPart.gameObject.SetActive(true);
        }

        if (currentShell == null)
        {
            Rescale();
        }

        eatEventEmitter.start();
        eatEventEmitter.release();
    }

    public void Rescale()
    {
        var initScale = transform.localScale.sqrMagnitude;
        transform.localScale = Vector3.one * Size.sizeScale[size];
        if (initScale < transform.localScale.sqrMagnitude)
            smokePart.Play();

        rb.mass = 2 + (size * 2);
    }

    public void SetPlayer(int i)
    {
        GetComponent<PlayerInput>().playerString = $"P{i + 1}_";
        color = Player.colors[TitleController.playerColors[i]];
        colorSprites.ForEach(sprite => sprite.color = color);
    }

    public void Disable()
    {
        SetVisibility(false);
    }

    public void Enable(Vector2? position = null)
    {
        SetVisibility(true);
        if (position != null)
            transform.position = position.Value;
    }

    public void Infect(float duration, float totalDamage, int ticks = 10)
    {
        poison = new Poison(this, duration, totalDamage, ticks);
    }

    public void SetVisibility(bool isVisible)
    {
        if (this.isVisible == isVisible)
            return;

        this.isVisible = isVisible;

        foreach (SpriteRenderer sprite in colorSprites)
            sprite.color = !isVisible ? UnityEngine.Color.clear : color;
    }

    public void EnterShell()
    {
        bubblesParticles.SetActive(true);
        enterShellEventEmitter.start();
        enterShellEventEmitter.release();
    }

    public void Shoot()
    {
        currentShell?.Shoot(aimDirection);

        animator.SetTrigger("shoot"); //TODO
        //if (currentShell != null && currentShell.canShoot)
        //{
        //    animator.SetTrigger("shoot");
        //}
    }

    public void Sacrifice()
    {
        if (currentShell != null)
        {
            currentShell.Sacrifice(aimDirection);
            exitShellEventEmitter.start();
            Rescale();
            disconfortPart.gameObject.SetActive(false);
        }
    }

    public void Move(float h, float v)
    {
        rb.velocity = new Vector2(h, v).normalized * speed * (currentShell != null ? 1 / (1.5f + currentShell.size) : 1);
        var isMoving = rb.velocity.magnitude > 0.1f;

        animator.SetBool("walking", isMoving);
        //animator.speed = isMoving ? 1 : (rb.velocity.magnitude / 12f);
        animator.SetFloat("speed", (rb.velocity.magnitude / 12f));
        sandPart.Emit((int)(rb.velocity.magnitude * 0.25f));
        if (h != 0)
        {
            sprite.transform.localScale = new Vector3(Mathf.Abs(sprite.transform.localScale.x) * (h > 0 ? -1 : 1), sprite.transform.localScale.y, sprite.transform.localScale.z);
        }

        if (isMoving)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            walkEventEmitter.getPlaybackState(out state);
            //walkEventEmitter.setPitch(0.5f + animator.speed);

            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                walkEventEmitter.start();
            }

        }
        else
        {
            walkEventEmitter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void Aim(float h, float v, float aim_h, float aim_v)
    {
        Vector2 dir = new Vector2(aim_h, aim_v).normalized;
        if (dir.sqrMagnitude == 0)
        {
            dir = new Vector2(h, v).normalized;
            if (dir.sqrMagnitude == 0)
            {
                dir = aimDirection;
            }
        }

        aimDirection = dir;
    }

    public void TakeDamage(float damage)
    {
        if (currentShell == null)
        {
            if (hp <= 0)
                Die();
            else
                hp--;
        }
        else if (canTakeDamage)
        {
            currentShell.TakeDamage(damage);
            if (!isPoisoned)
                Invulnerability(afterHitInvulnerabilityDuration);
        }
    }

    public void Die()
    {
        for (int i = 0; i < meatsCollected + 2; i++)
        {
            var meat = Instantiate(meatPrefab);
            meat.meatSprite.sprite = meatSprites.GetRandom();
            meat.meatSprite.color = color;
            //meat.transform.localScale = transform.localScale;
            meat.transform.position = transform.position;
            meat.Go(transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * 2.5f));
        }
        OnDie(this);
        deathEventEmitter.start();
        gameObject.SetActive(false);
    }

    public void SetImmunity(bool isImmune)
    {
        canTakeDamage = !isImmune;
    }

    public void Invulnerability(float duration)
    {
        canTakeDamage = false;
        this.DelayedAction(duration, () => canTakeDamage = true);
    }

    private void OnDestroy()
    {
        walkEventEmitter.release();
        walkEventEmitter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

}
