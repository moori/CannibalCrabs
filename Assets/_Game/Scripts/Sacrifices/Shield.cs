using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float duration;
    public Player owner;
    public ParticleSystem explosionPart;
    private float startTime;
    [EventRef]
    public string shieldEvent;

    public void Activate(Player player)
    {
        owner = player;
        owner.hp = 99;
        transform.SetParent(owner.transform);
        transform.localPosition = Vector3.zero;
        startTime = Time.time;
        RuntimeManager.PlayOneShot(shieldEvent);
    }

    public void Update()
    {
        if (owner.hp < 99)
        {
            Explode();
        }
        if (Time.time - startTime > duration)
            Explode();
    }

    private void Explode()
    {
        owner.hp = 0;
        explosionPart.transform.SetParent(null);
        explosionPart.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
