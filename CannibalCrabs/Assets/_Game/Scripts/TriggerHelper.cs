using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHelper : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnterAction = (col) => { };
    public Action<Collider2D> OnTriggerStayAction = (col) => { };
    public Action<Collider2D> OnTriggerExitAction = (col) => { };
    public Collider2D collider2DD;

    private void Awake()
    {
        collider2DD = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnterAction(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStayAction(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExitAction(other);
    }
}
