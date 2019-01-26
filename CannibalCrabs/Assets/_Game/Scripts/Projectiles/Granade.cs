using System.Collections;
using UnityEngine;

public class Granade : Projectile
{
    public AnimationCurve spriteScaleCurve;
    public SpriteRenderer granadeSprite;
    public Vector2 target;
    private bool isTraveling;
    private Vector2 startPos;

    public override void Go(Player owner, Vector2 direction)
    {
        isTraveling = true;
        rb.velocity = direction * speed;
        startPos = transform.position;
        StartCoroutine(TravelRoutine());
    }

    IEnumerator TravelRoutine()
    {
        while (isTraveling)
        {
            var percent = (Vector2.Distance(target, startPos) - Vector2.Distance(target, (Vector2)transform.position)) / Vector2.Distance(target, startPos);
            granadeSprite.transform.localScale = Vector2.one * (1 + spriteScaleCurve.Evaluate(percent) * .2f);
            granadeSprite.transform.localPosition = Vector2.up * spriteScaleCurve.Evaluate(percent) * 0.9f;
            sprite.transform.localScale = Vector2.one * (1f - spriteScaleCurve.Evaluate(percent) * 0.1f);
            if (percent >= 0.95f)
            {
                rb.velocity = Vector2.zero;
                isTraveling = false;
                Destroy(gameObject, 0.5f);
                sprite.enabled = false;
                granadeSprite.enabled = false;
            }
            yield return new WaitForFixedUpdate();
        }
        GetComponent<Collider2D>().enabled = true;
    }

    public override void Hit(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == owner)
                return;

            player.TakeDamage(damage);
        }

    }

}
