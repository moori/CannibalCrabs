using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameShell : Shell
{
    public NinjaBomb ninjaBombPrefab;
    public ParticleSystem FlameParticles;
    public Healthbar flameAmmoBar;
    public float flameAmmoDuration;
    public float flameAmmoReload;
    public bool canFlame = true;
    public float damageInterval;
    public float damage;
    public TriggerHelper damageTrigger;
    public float turnSpeed;

    private bool isShooting;

    public override void Awake()
    {
        base.Awake();
        damageTrigger.OnTriggerEnterAction = OnDamageTriggerEnter;
        damageTrigger.enabled = false;
    }

    private void Start()
    {
        damageTrigger.collider2DD.enabled = false;
    }

    private void OnDamageTriggerEnter(Collider2D obj)
    {
        Debug.Log(obj.name);
        if (obj.CompareTag("Player"))
        {
            Player player = obj.gameObject.GetComponent<Player>();
            if (player != owner)
                player.TakeDamage(damage);
        }
    }

    public override void Shoot(Vector2 direction)
    {
        if (!canFlame)
            return;

        FlameParticles.Play();
        timeLastShot = Time.time;
        shootEventEmitter.start();
    }

    public override void EnterShell(Player player)
    {
        base.EnterShell(player);

        flameAmmoBar.canvasGroup.alpha = 1;
        damageTrigger.enabled = true;
        StartCoroutine(DamageRoutine());
    }

    private void Update()
    {
        if (!owner)
            return;

        float percent = flameAmmoBar.fillImage.fillAmount;
        isShooting = !(Time.time - timeLastShot > Time.deltaTime * 2);

        if (!isShooting)
        {
            //reload
            percent += (Time.deltaTime) / flameAmmoReload;
            FlameParticles.Stop();
        }
        else
        {
            //shooting
            percent -= (Time.deltaTime / flameAmmoDuration);
        }

        if (percent <= 0.05f && canFlame)
        {
            canFlame = false;
            flameAmmoBar.fillImage.color = Color.red;
            this.DelayedAction(2f, () =>
            {
                canFlame = true;
                flameAmmoBar.fillImage.color = Color.green;
            });
        }

        FlameParticles.transform.up = Vector3.Lerp(damageTrigger.transform.up, owner.aimDirection, Time.deltaTime * turnSpeed);
        damageTrigger.transform.up = Vector3.Lerp(damageTrigger.transform.up, owner.aimDirection, Time.deltaTime * turnSpeed);

        flameAmmoBar.fillImage.fillAmount = percent;
    }

    IEnumerator DamageRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => isShooting);
            yield return new WaitForSeconds(damageInterval);
            damageTrigger.collider2DD.enabled = isShooting;
            yield return new WaitForFixedUpdate();
            damageTrigger.collider2DD.enabled = false;
        }
    }

    public override void Sacrifice(Vector2 direction)
    {
        base.Sacrifice(direction);
        NinjaBomb ninjaBomb = Instantiate(ninjaBombPrefab);
        ninjaBomb.Activate(owner, transform.position);
    }
}
