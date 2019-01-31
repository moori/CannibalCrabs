using System.Collections;
using UnityEngine;

public class Granade : Projectile
{
    public AnimationCurve spriteScaleCurve;
    public SpriteRenderer granadeSprite;
    public float range;
    public Vector2 target;
    private bool isTraveling;
    private Vector2 startPos;


    public override void Go(Player owner, Vector2 direction)
    {
        isTraveling = true;
        rb.velocity = direction * speed;
        startPos = transform.position;
        target = startPos + direction * range;
        damage += owner.size * 0.25f;
        granadeSprite.color = owner.color;
        StartCoroutine(TravelRoutine());
    }

    IEnumerator TravelRoutine()
    {
        while (isTraveling)
        {
            var percent = (Vector2.Distance(target, startPos) - Vector2.Distance(target, (Vector2)transform.position)) / Vector2.Distance(target, startPos);
            granadeSprite.transform.localScale = Vector2.one * (1 + spriteScaleCurve.Evaluate(percent) * .2f);
            granadeSprite.transform.localPosition = Vector2.up * spriteScaleCurve.Evaluate(percent) * 3f;
            sprite.transform.localScale = Vector2.one * (3f - spriteScaleCurve.Evaluate(percent) * 1.5f);
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

        hitPart.gameObject.SetActive(true);
        hitPart.gameObject.transform.SetParent(null);

        hitEventEmitter.start();

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == owner)
                return;

            player.TakeDamage(damage);
        }
        //else if (collision.CompareTag("Shell"))
        //{
        //    Shell shell = collision.GetComponent<Shell>();
        //    shell.Push((collision.transform.position - transform.position).normalized, damage / 2);
        //    shell.TakeDamage(damage);
        //}

    }

}
