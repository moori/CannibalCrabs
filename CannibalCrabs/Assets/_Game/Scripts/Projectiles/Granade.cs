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
            var percent = Vector2.Distance(target, (Vector2)transform.position) / Vector2.Distance(target, startPos);
            granadeSprite.transform.localScale = Vector2.one * spriteScaleCurve.Evaluate(percent) * 0.5f;

            isTraveling = percent < 0.95f;
            yield return new WaitForEndOfFrame();
        }

        GetComponent<Collider2D>().enabled = true;
    }

}
